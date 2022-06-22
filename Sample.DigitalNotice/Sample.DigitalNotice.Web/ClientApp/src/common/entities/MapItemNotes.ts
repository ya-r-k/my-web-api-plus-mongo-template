import { Note } from './Note'
import { NoteGroup } from './NoteGroup'

export type MapItemNotes = {
  templateItemNumber: number
  notes: Note[]
  noteGroups: NoteGroup[]
}
