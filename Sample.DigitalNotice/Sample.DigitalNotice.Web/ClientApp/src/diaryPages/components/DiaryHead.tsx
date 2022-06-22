import React from 'react'
import { DiaryHeadProps as Props } from '../props/DiaryHeadProps'
import DiaryEditHead from './DiaryEditHead'
import DiaryInfoHead from './DiaryInfoHead'

export default function DiaryHead(props: Props) {
  return (
    <React.Fragment>
      {props.isEditing && <DiaryEditHead {...props} />}
      {!props.isEditing && <DiaryInfoHead {...props} />}
    </React.Fragment>
  )
}
