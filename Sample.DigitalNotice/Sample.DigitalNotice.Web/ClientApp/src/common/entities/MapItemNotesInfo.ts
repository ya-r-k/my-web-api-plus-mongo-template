import { NoteGroupInfo } from './NoteGroupInfo'
import { NoteInfo } from './NoteInfo'

export type MapItemNotesInfo = {
  templateItemNumber: number
  notes: NoteInfo[]
  noteGroups: NoteGroupInfo[]
}
