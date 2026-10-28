# Antiyoy C# map-generation port — audit and fixes

## Main changes

- Removed `Hex.getPos()` and `FieldManager.getHexByPos()` from the map-generation path. `Hex.index1/index2` are now the canonical coordinates.
- `MapGenerator` no longer uses `PointYio`, `Yio`, screen/world positions, or `FieldManager.hexSize` for generation.
- Bounds are expressed as array-index margins. The level-size enum controls the relative margins, rather than pixel/world dimensions.
- Island centres are stored as `Hex` references. Closest-island distances and road drawing use axial hex-grid coordinates.
- Fixed `relocateMap`: the original C# port checked `deltaY < fHeight` instead of `destY < fHeight`, and shifting cells in-place could duplicate/drop active cells. The corrected method snapshots source activity/fractions before applying the offset.
- Reinitializing the field matrix clears stale `activeHexes` and `solidObjects` references.
- Island centres and road links are cleared on every failed generation attempt; added a finite retry limit and validation for fields with no cells inside bounds.
- Replaced `Mathf.Abs` in the generator with `Math.Abs`, so the generation algorithm itself has no Unity world-coordinate dependency.

## Coordinate convention

`index1/index2` are axial-style array coordinates. Neighbours remain:
`(+1,0), (0,+1), (-1,+1), (-1,0), (0,-1), (+1,-1)`.
Roads are rasterized between island centres in this coordinate system; map rendering can independently map indices to Unity world positions.

## Integration notes

- Replace the matching files in `Ported/` in your project.
- `PointYio.cs` and `Yio.cs` are included only as existing project dependencies; the updated map generators no longer use them.
- This is a source-level audit and structural check, not a Unity Editor build. Random sequences will not match Java exactly because `System.Random` differs from `java.util.Random`.
