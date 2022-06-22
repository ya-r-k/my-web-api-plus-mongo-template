import { NoteStatus } from '../../common/enums/NoteStatus'
import { NoteCardProps as Props } from '../props/NoteCardProps'
import { removeNote, startEditNote } from '../redux/mapSlice'

export default function NoteInfoCard({
  mapItemIndex,
  templateItemIndex,
  noteIndex,
  isEditingNote,
  noteInfo,
  dispatch,
}: Props) {
  return (
    <div {...{ className: 'note-card' }}>
      <div {...{ className: 'title-block' }}>
        <h3>{noteInfo.name}</h3>
        {!isEditingNote && (
          <div {...{ className: 'actions-block' }}>
            <button
              {...{
                className: 'edit',
                onClick: () =>
                  dispatch(
                    startEditNote({
                      mapItemIndex,
                      templateItemIndex,
                      noteIndex,
                    })
                  ),
              }}
            >
              Edit note
            </button>
            <button
              {...{
                className: 'remove',
                onClick: () =>
                  dispatch(
                    removeNote({ mapItemIndex, templateItemIndex, noteIndex })
                  ),
              }}
            >
              Delete note
            </button>
          </div>
        )}
      </div>
      <p>{noteInfo.content}</p>
      {noteInfo.status === NoteStatus.Draft && <p>Draft</p>}
      {noteInfo.status === NoteStatus.Created && noteInfo.createdDate && (
        <p>Created: {new Date(noteInfo.createdDate).toLocaleString()}</p>
      )}
      {noteInfo.status === NoteStatus.Edited && noteInfo.editedDate && (
        <p>Edited: {new Date(noteInfo.editedDate).toLocaleString()}</p>
      )}
    </div>
  )
}
