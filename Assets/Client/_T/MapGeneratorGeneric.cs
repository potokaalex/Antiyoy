using System;
using System.Collections.Generic;
using Client.Project.Map;

namespace Antiyoy.Gameplay
{
    public class MapGeneratorGeneric : MapGenerator {

        bool flagNeutrals;

        public MapGeneratorGeneric(GameController gameController) : base(gameController) 
        {
        }

        public override void generateMap(System.Random random, Hex[][] field) {
            setValues(random, field);

            beginGeneration();

            createLand();
            addTrees();

            while (!hasGreenProvince()) {
                genericBalance();
            }

            endGeneration();
        }


        // not actually needed right now
        private bool hasGreenProvince() {
            foreach (Hex activeHex in getActiveHexes()) {
                if (activeHex.fraction != 0) continue; //что значит activeHex.fraction != 0 т.е. 0 это без инициализации и мы ловим такие клетки?
                if (activeHex.numberOfFriendlyHexesNearby() < 2) continue;

                return true;
            }

            return false;
        }


        private void genericBalance() {
            makeAllActiveHexesNeutral();
            spawnProvinces();
            cutProvincesToSmallSizes();
            makeSingleHexesIntoProvinces();
            applyBalanceMeasures();
        }


        private void makeAllActiveHexesNeutral() {
            foreach (Hex activeHex in getActiveHexes()) {
                activeHex.fraction = GameRules.NEUTRAL_FRACTION;
            }

            flagNeutrals = true;
        }


        private void spawnProvinces() {
            int quantity = getProvincesQuantity();
            for (int i = 0; i < quantity; i++) {
                for (int fraction = 0; fraction < GameRules.fractionsQuantity; fraction++) {
                    Hex hex = findGoodPlaceForNewProvince();
                    if (hex == null) continue;

                    hex.setFraction(fraction);
                    spawnProvince(hex, 2);
                }
            }
        }


        private void makeSingleHexesIntoProvinces() {
            foreach (Hex activeHex in getActiveHexes()) {
                if (activeHex.isNeutral()) continue;
                if (activeHex.numberOfFriendlyHexesNearby() > 0) continue;
                int c = 3;
                for (int i = 0; i < 6; i++) {
                    Hex adjHex = activeHex.getAdjacentHex(i);
                    if (!adjHex.active || !adjHex.isNeutral()) continue;
                    adjHex.fraction = activeHex.fraction;
                    c--;
                    if (c == 0) break;
                }
            }
        }


        
        protected override void applyBalanceMeasures() {
            double[] changesArray = getChangesArray();
            if (changesArray == null) return;
            applyChanges(changesArray);
        }


        private double[] getChangesArray() {
            switch (GameRules.fractionsQuantity) {
                default:
                    return null;
                case 2:
                    return new double[] {-0.4, 0.3};
                case 3:
                    return new double[] {-0.5, 0.2, 0.4};
                case 4:
                    return new double[] {-0.6, 0.12, 0.3, 0.6};
                case 5:
                    return new double[] {-0.5, -0.35, 0.15, 0.32, 0.5};
                case 6:
                    return new double[] {-0.6, -0.75, 0.15, 0.25, 0.4, 0.6};
                case 7:
                    return new double[] {-0.4, -0.9, 0.2, 0.3, 0.45, 0.6, 0.8};
            }
        }


        private void applyChanges(double[] changes) {
            for (int i = 0; i < changes.Length; i++) {
                if (changes[i] == 0) continue;

                if (changes[i] > 0) {
                    giveAdvantageToPlayer(i, changes[i]);
                } else {
                    giveDisadvantageToPlayer(i, changes[i]);
                }
            }
        }


        private void newButBadBalanceMeasures() {
            double adv_start = 0.3;
            double adv_delta = adv_start / GameRules.fractionsQuantity;
            adv_delta /= 2;
            adv_delta += 0.01;
            double dis_start = 0.25;
            double dis_delta = 0.15;

            double adv = adv_start;
            for (int fraction = GameRules.fractionsQuantity - 1; fraction >= 0; fraction--) {
                giveAdvantageToPlayer(fraction, adv);
                adv -= adv_delta;
                if (adv <= 0) break;
            }

            double dis = dis_start;
            for (int fraction = 0; fraction < GameRules.fractionsQuantity; fraction++) {
                giveDisadvantageToPlayer(fraction, dis);
                dis += dis_delta;
                if (fraction > GameRules.fractionsQuantity / 3) break;
            }
        }


        
        protected override void decreaseProvince(List<Hex> provinceList, double power) {
            int num = (int) (power * provinceList.Count);
            for (int i = 0; i < num; i++) {
                Hex hex = findHexToExcludeFromProvince(provinceList);
                provinceList.Remove(hex);
                hex.fraction = GameRules.NEUTRAL_FRACTION;
            }
        }


        
        protected override void spawnProvince(Hex spawnHex, int startingPotential) {
            spawnHex.genPotential = startingPotential;
            List<Hex> propagationList = new List<Hex>();
            propagationList.Add(spawnHex);
            while (propagationList.Count > 0) {
                Hex hex = propagationList[0];
                propagationList.RemoveAt(0);
                if (random.Next(startingPotential) > hex.genPotential) continue;
                hex.fraction = spawnHex.fraction;
                if (hex.genPotential == 0) continue;
                for (int i = 0; i < 6; i++) {
                    Hex adjHex = hex.getAdjacentHex(i);
                    if (!propagationList.Contains(adjHex) && adjHex.active && adjHex.fraction == GameRules.NEUTRAL_FRACTION) {
                        adjHex.genPotential = hex.genPotential - 1;
                        propagationList.Add(adjHex);
                    }
                }
            }
        }


        
        protected override void reduceProvinceSize(List<Hex> provinceList) {
            while (provinceList.Count > SMALL_PROVINCE_SIZE) {
                Hex hex = findHexToExcludeFromProvince(provinceList);
                provinceList.Remove(hex);
                hex.fraction = GameRules.NEUTRAL_FRACTION;
            }
        }


        
        protected override bool activateHex(Hex hex, int fraction) {
            if (hex.active) return false;
            hex.active = true;
            hex.setFraction(GameRules.NEUTRAL_FRACTION);
            getActiveHexes().Add(hex);
            return true;
        }


        Hex findGoodPlaceForNewProvince() {
            if (allHexesAreNeutral()) {
                return getRandomFreeHex();
            }

            prepareHexesToFindNewProvincePlace();

            bool expanded;
            int step = 0;
            while (true) {
                expanded = false;
                foreach (Hex hex in getActiveHexes()) {
                    if (hex.moveZoneNumber != step) continue;
                    for (int dir = 0; dir < 6; dir++) {
                        Hex adj = hex.getAdjacentHex(dir);
                        if (adj == null) continue;
                        if (adj.isNullHex()) continue;
                        if (!adj.active) continue;
                        if (adj.moveZoneNumber != -1) continue;

                        adj.moveZoneNumber = step + 1;
                        expanded = true;
                    }
                }

                if (!expanded) break;
                step++;
            }

            return findHexWithHighestMoveZoneNumber();
        }


        private Hex findHexWithHighestMoveZoneNumber() {
            Hex result = null;

            foreach (Hex hex in getActiveHexes()) {
                if (result == null || hex.moveZoneNumber > result.moveZoneNumber) {
                    result = hex;
                }
            }

            return result;
        }


        private void prepareHexesToFindNewProvincePlace() {
            foreach (Hex hex in getActiveHexes()) {
                if (hex.isNeutral()) {
                    hex.moveZoneNumber = -1;
                    continue;
                }

                hex.moveZoneNumber = 0;
            }
        }


        private List<Hex> getActiveHexes() {
            return gameController.fieldManager.activeHexes;
        }


        private bool allHexesAreNeutral() {
            if (!flagNeutrals) return false;

            foreach (Hex activeHex in getActiveHexes()) {
                if (activeHex.isNeutral()) continue;

                flagNeutrals = false;
                return false;
            }

            return true;
        }


        protected Hex getRandomFreeHex() {
            return getActiveHexes()[random.Next(getActiveHexes().Count)];
        }


        protected int getProvincesQuantity() {
            //if (GameRules.genProvinces > 0) return GameRules.genProvinces;

            switch (gameController.levelSizeManager.levelSize) {
                default:
                case LevelSize.Small:
                    return 1;
                case LevelSize.Medium:
                    return 2;
                case LevelSize.Big:
                    return 3;
                case LevelSize.Huge:
                    return 4;
            }
        }
    }
}
