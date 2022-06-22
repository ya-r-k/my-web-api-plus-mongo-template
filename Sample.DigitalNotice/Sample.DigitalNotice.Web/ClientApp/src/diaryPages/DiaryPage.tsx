import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useParams } from 'react-router-dom'
import { NoteInfo } from '../common/entities/NoteInfo'
import NotFoundPage from '../commonPages/NotFoundPage'
import Page from '../commonPages/Page'
import { RootState } from '../store/store'
import DiaryHead from './components/DiaryHead'
import NoteCard from './components/NoteCard'
import { DiaryPageParams } from './params/DiaryPageParams'
import {
  addNote,
  clearNotesInfo,
  setDiary,
  setNotesInfo,
  setResponseInfo,
} from './redux/diarySlice'

export default function DiaryPage() {
  const data = useSelector((state: RootState) => state.diaryReducer)
  const dispatch = useDispatch()
  const { id } = useParams<DiaryPageParams>()

  useEffect(() => {
    dispatch(clearNotesInfo())

    fetch(`https://localhost:44488/api/diaries/${id}`).then((response) => {
      if (response.status === 200) {
        response.json().then((result) => dispatch(setDiary(result)))
      }

      dispatch(
        setResponseInfo({
          status: response.status,
          statusText: response.statusText,
        })
      )
    })
  }, [id])

  useEffect(() => {
    if (data.notes.length === 0 && data.diary) {
      const notes: NoteInfo[] = data.diary.notes
        .map((item, index) => {
          return { ...item, index, isSaved: true, isEditing: false }
        })
        .reverse()

      dispatch(setNotesInfo(notes))
    }
  }, [data.diary])

  return (
    <React.Fragment>
      {(data.responseInfo?.status === 404 ||
        data.responseInfo?.status === 400) && <NotFoundPage />}
      {data.diary && (
        <Page {...{ title: 'Diary' }}>
          <DiaryHead
            {...{ diary: data.diary, dispatch, isEditing: data.isEditing }}
          />
          <div {...{ className: 'notes-list' }}>
            <h2>Notes</h2>
            {!data.isEditingNote && (
              <button
                {...{
                  className: 'add',
                  onClick: () => {
                    dispatch(addNote())
                  },
                }}
              >
                + Add note
              </button>
            )}
            {data.notes.map((item, index) => {
              return (
                <NoteCard
                  {...{
                    noteInfo: item,
                    isEditingNote: data.isEditingNote,
                    dispatch,
                    key: index,
                    index,
                  }}
                />
              )
            })}
          </div>
        </Page>
      )}
    </React.Fragment>
  )
}
