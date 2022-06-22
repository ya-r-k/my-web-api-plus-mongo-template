import { AnyAction, Dispatch } from '@reduxjs/toolkit'
import { Diary } from '../../common/entities/Diary'
import { NoticeStatus } from '../../common/enums/NoticeStatus'

export interface DiaryHeadProps {
  diary: Diary
  isEditing: boolean
  dispatch: Dispatch<AnyAction>
}
