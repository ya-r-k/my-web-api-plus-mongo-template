import { NoteStatus } from '../../common/enums/NoteStatus'
import { NoteCardProps as Props } from '../props/NoteCardProps'
import {
  removeNote,
  saveCreatedNote,
  saveDraftNote,
  saveEditedNote,
  setNoteContent,
  setNoteName,
} from '../redux/diarySlice'

export default function NoteEditCard({ index, noteInfo, dispatch }: Props) {
  return (
    <div {...{ className: 'note-card' }}>
      <div {...{ className: 'title-block' }}>
        <input
          {...{
            value: noteInfo.name,
            placeholder: 'Note name',
            onChange: (e) => {
              dispatch(
                setNoteName({
                  name: e.target.value,
                  index,
                })
              )
            },
          }}
        />
        <div {...{ className: 'actions-block' }}>
          {noteInfo.status === NoteStatus.Draft && (
            <button
              {...{
                className: 'submit',
                onClick: () => {
                  dispatch(saveDraftNote({ index }))
                },
              }}
            >
              Save note as draft
            </button>
          )}
          {noteInfo.status === NoteStatus.Draft && (
            <button
              {...{
                className: 'add',
                onClick: () => {
                  dispatch(saveCreatedNote({ index }))
                },
              }}
            >
              Save note
            </button>
          )}
          {(noteInfo.status === NoteStatus.Created ||
            noteInfo.status === NoteStatus.Edited) && (
            <button
              {...{
                className: 'edit',
                onClick: () => {
                  dispatch(saveEditedNote({ index }))
                },
              }}
            >
              Save note
            </button>
          )}
          <button
            {...{
              className: 'remove',
              onClick: () => {
                dispatch(removeNote({ index }))
              },
            }}
          >
            Delete note
          </button>
        </div>
      </div>
      <textarea
        {...{
          value: noteInfo.content,
          placeholder: 'Note content',
          onChange: (e) => {
            dispatch(setNoteContent({ content: e.target.value, index }))
          },
        }}
      />
    </div>
  )
}
