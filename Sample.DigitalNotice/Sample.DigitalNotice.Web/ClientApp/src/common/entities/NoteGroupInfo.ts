import { NoteInfo } from './NoteInfo'

export type NoteGroupInfo = {
  name: string
  notes: NoteInfo[]
  isEditing: boolean
}
