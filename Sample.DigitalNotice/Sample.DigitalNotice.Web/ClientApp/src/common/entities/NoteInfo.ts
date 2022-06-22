import { NoteStatus } from '../enums/NoteStatus'

export type NoteInfo = {
  name: string
  content: string
  createdDate?: string
  editedDate?: string
  status: NoteStatus
  isSaved: boolean
  isEditing: boolean
}
