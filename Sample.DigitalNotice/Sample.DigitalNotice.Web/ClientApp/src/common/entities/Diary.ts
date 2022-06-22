import { NoticeStatus } from '../enums/NoticeStatus'
import { NoticeType } from '../enums/NoticeType'
import { Note } from './Note'

export type Diary = {
  id?: string
  name: string
  description: string
  createdDate?: string
  status: NoticeStatus
  type: NoticeType
  notes: Note[]
}
