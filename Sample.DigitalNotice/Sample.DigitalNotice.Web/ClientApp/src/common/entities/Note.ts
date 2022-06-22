import { NoteStatus } from '../enums/NoteStatus'

export type Note = {
  name: string
  content: string
  createdDate?: string
  editedDate?: string
  status: NoteStatus
}
