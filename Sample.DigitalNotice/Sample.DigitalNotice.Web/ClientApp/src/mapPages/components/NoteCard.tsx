import React from 'react'
import NoteEditCard from './NoteEditCard'
import NoteInfoCard from './NoteInfoCard'
import { NoteCardProps as Props } from '../props/NoteCardProps'

export default function NoteCard(props: Props) {
  return (
    <React.Fragment>
      {props.noteInfo.isEditing && <NoteEditCard {...props} />}
      {!props.noteInfo.isEditing && <NoteInfoCard {...props} />}
    </React.Fragment>
  )
}
