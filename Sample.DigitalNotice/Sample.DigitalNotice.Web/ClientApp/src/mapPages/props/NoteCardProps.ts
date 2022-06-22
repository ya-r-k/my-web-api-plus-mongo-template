import { AnyAction, Dispatch } from '@reduxjs/toolkit'
import { NoteInfo } from '../../common/entities/NoteInfo'

export interface NoteCardProps {
  mapItemIndex: any
  templateItemIndex: any
  noteIndex: any
  isEditingNote: boolean
  noteInfo: NoteInfo
  dispatch: Dispatch<AnyAction>
}
