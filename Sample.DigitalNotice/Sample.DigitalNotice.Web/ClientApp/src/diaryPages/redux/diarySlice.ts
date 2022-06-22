import type { PayloadAction } from '@reduxjs/toolkit'
import { createSlice } from '@reduxjs/toolkit'
import { Diary } from '../../common/entities/Diary'
import { NoteInfo } from '../../common/entities/NoteInfo'
import { NoteStatus } from '../../common/enums/NoteStatus'
import { NoticeStatus } from '../../common/enums/NoticeStatus'

const initialState: {
  diary?: Diary
  isEditing: boolean
  isEditingNote: boolean
  notes: NoteInfo[]
  responseInfo?: { status: number; statusText: string }
} = {
  notes: [],
  isEditing: false,
  isEditingNote: false,
}

export const diarySlice = createSlice({
  name: 'diary',
  initialState,
  reducers: {
    setDiary: (state, action: PayloadAction<Diary>) => {
      state.diary = action.payload
    },
    setResponseInfo: (
      state,
      action: PayloadAction<{ status: number; statusText: string }>
    ) => {
      state.responseInfo = action.payload
    },
    setNotesInfo: (state, action: PayloadAction<NoteInfo[]>) => {
      state.notes = action.payload
    },
    clearNotesInfo: (state) => {
      state.notes = []
    },
    setNoteName: (
      state,
      action: PayloadAction<{ name: string; index: number }>
    ) => {
      state.notes[action.payload.index].name = action.payload.name
    },
    setNoteContent: (
      state,
      action: PayloadAction<{ content: string; index: number }>
    ) => {
      state.notes[action.payload.index].content = action.payload.content
    },
    addNote: (state) => {
      state.isEditingNote = true

      state.notes.unshift({
        name: '',
        content: '',
        status: NoteStatus.Draft,
        isSaved: false,
        isEditing: true,
      })
    },
    removeNote: (state, action: PayloadAction<{ index: number }>) => {
      if (!state.diary) {
        return
      }

      const note = state.notes[action.payload.index]

      state.isEditingNote = false

      state.notes.splice(action.payload.index, 1)

      const index = state.notes.length - action.payload.index

      if (note.isSaved && index < state.diary.notes.length) {
        state.diary.notes.splice(index, 1)
      }
    },
    setDiaryName: (state, action: PayloadAction<{ name: string }>) => {
      if (state.diary) {
        state.diary.name = action.payload.name
      }
    },
    setDiaryDescription: (
      state,
      action: PayloadAction<{ description: string }>
    ) => {
      if (state.diary) {
        state.diary.description = action.payload.description
      }
    },
    saveDraftDiary: (state) => {
      if (!state.diary) {
        return
      }

      state.isEditing = false
      state.diary.status = NoticeStatus.Draft
    },
    saveCreatedDiary: (state) => {
      if (!state.diary) {
        return
      }

      state.isEditing = false
      state.diary.status = NoticeStatus.Created
    },
    startEditNote: (state, action: PayloadAction<{ index: number }>) => {
      state.isEditingNote = true
      state.notes[action.payload.index].isEditing = true
    },
    startEditDiaryCommonInfo: (state) => {
      state.isEditing = true
    },
    saveDraftNote: (state, action: PayloadAction<{ index: number }>) => {
      if (!state.diary) {
        return
      }

      const editedNote = state.notes[action.payload.index]

      if (!editedNote.isSaved) {
        editedNote.isSaved = true
      }

      state.isEditingNote = false
      editedNote.isEditing = false

      const index = state.notes.length - action.payload.index - 1

      if (index >= state.diary.notes.length) {
        const diaryNote = {
          name: editedNote.name,
          content: editedNote.content,
          status: NoteStatus.Draft,
        }

        state.diary?.notes.push(diaryNote)
      } else {
        const diaryNote = state.diary?.notes[index]

        diaryNote.name = editedNote.name
        diaryNote.content = editedNote.content
      }

      state.notes.sort((first, second) =>
        first.isSaved && first.isSaved === !second.isSaved ? 1 : 2
      )
    },
    saveCreatedNote: (state, action: PayloadAction<{ index: number }>) => {
      if (!state.diary) {
        return
      }

      const editedNote = state.notes[action.payload.index]

      if (!editedNote.isSaved) {
        editedNote.isSaved = true
      }

      state.isEditingNote = false
      editedNote.isEditing = false

      const index = state.notes.length - action.payload.index - 1
      const diaryNote =
        index >= state.diary.notes.length
          ? { name: '', content: '', status: NoteStatus.Draft }
          : state.diary?.notes[index]

      diaryNote.name = editedNote.name
      diaryNote.content = editedNote.content
      diaryNote.status = NoteStatus.Created
      diaryNote.createdDate = new Date().toISOString()

      if (index >= state.diary.notes.length) {
        state.diary?.notes.push(diaryNote)
      }

      editedNote.createdDate = diaryNote.createdDate
      editedNote.status = diaryNote.status
    },
    saveEditedNote: (state, action: PayloadAction<{ index: number }>) => {
      if (!state.diary) {
        return
      }

      state.isEditingNote = false
      const editedNote = state.notes[action.payload.index]

      const index = state.notes.length - action.payload.index - 1
      const diaryNote = state.diary?.notes[index]

      diaryNote.name = editedNote.name
      diaryNote.content = editedNote.content
      diaryNote.status = NoteStatus.Edited
      diaryNote.editedDate = new Date().toISOString()

      editedNote.editedDate = diaryNote.editedDate
      editedNote.status = diaryNote.status
      editedNote.isEditing = false
    },
  },
})

export const {
  setDiary,
  setResponseInfo,
  setNotesInfo,
  clearNotesInfo,
  setNoteName,
  setNoteContent,
  addNote,
  removeNote,
  setDiaryName,
  setDiaryDescription,
  saveDraftDiary,
  saveCreatedDiary,
  startEditDiaryCommonInfo,
  startEditNote,
  saveDraftNote,
  saveCreatedNote,
  saveEditedNote,
} = diarySlice.actions

export default diarySlice.reducer
