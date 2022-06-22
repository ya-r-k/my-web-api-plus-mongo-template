import { MapItem } from '../../common/entities/MapItem'

export function getMapItemNotes(mapItem: MapItem) {
  return new Map(mapItem.notes.map((item) => [item.templateItemNumber, item]))
}
