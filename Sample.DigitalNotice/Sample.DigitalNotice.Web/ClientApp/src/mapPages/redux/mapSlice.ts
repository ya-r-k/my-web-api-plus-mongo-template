import type { PayloadAction } from '@reduxjs/toolkit'
import { createSlice } from '@reduxjs/toolkit'
import { Map } from '../../common/entities/Map'
import { MapItemInfo } from '../../common/entities/MapItemInfo'
import { NoteStatus } from '../../common/enums/NoteStatus'
import { getMapItemNotes } from '../functions/getMapItemNotes'

const initialState: {
  map?: Map
  isEditing: boolean
  isEditingNote: boolean
  mapItems: MapItemInfo[]
  responseInfo?: { status: number; statusText: string }
} = {
  mapItems: [],
  isEditing: false,
  isEditingNote: false,
}

export const mapSlice = createSlice({
  name: 'map',
  initialState,
  reducers: {
    setMap: (state, action: PayloadAction<Map>) => {
      state.map = action.payload
    },
    setResponseInfo: (
      state,
      action: PayloadAction<{ status: number; statusText: string }>
    ) => {
      state.responseInfo = action.payload
    },
    setMapItemsInfo: (state, action: PayloadAction<MapItemInfo[]>) => {
      state.mapItems = action.payload
    },
    clearMapItemsInfo: (state) => {
      state.mapItems = []
    },
    addMapItem: (state) => {
      state.map?.items.push({
        name: '',
        notes: [],
      })
      state.mapItems.push({
        name: '',
        notes:
          state.map?.templateItems.map((item) => {
            return {
              templateItemNumber: item.number,
              notes: [],
              noteGroups: [],
            }
          }) || [],
      })
    },
    setNoteName: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
        name: string
      }>
    ) => {
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].notes[action.payload.noteIndex].name = action.payload.name
    },
    setNoteContent: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
        content: string
      }>
    ) => {
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].notes[action.payload.noteIndex].content = action.payload.content
    },
    addNote: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
      }>
    ) => {
      state.isEditingNote = true
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].notes.push({
        name: '',
        content: '',
        status: NoteStatus.Draft,
        isSaved: false,
        isEditing: true,
      })
    },
    removeNote: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      state.isEditingNote = true

      const note =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].notes[action.payload.noteIndex]

      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].notes.splice(action.payload.noteIndex, 1)

      if (note.isSaved) {
        const templateItemNumber =
          state.map.templateItems[action.payload.templateItemIndex].number

        const index = state.map.items[
          action.payload.mapItemIndex
        ].notes.findIndex(
          (item) => item.templateItemNumber === templateItemNumber
        )

        const mapItemNotes =
          state.map.items[action.payload.mapItemIndex].notes[index]

        mapItemNotes.notes.splice(action.payload.noteIndex, 1)

        if (
          mapItemNotes.notes.length === 0 &&
          mapItemNotes.noteGroups.length === 0
        ) {
          state.map.items[action.payload.mapItemIndex].notes.splice(index, 1)
        }
      }

      state.isEditingNote = false
    },
    startEditNote: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
      }>
    ) => {
      state.isEditingNote = true
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].notes[action.payload.noteIndex].isEditing = true
    },
    saveDraftNote: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      const editedNote =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].notes[action.payload.noteIndex]

      const newNote = {
        name: editedNote.name,
        content: editedNote.content,
        status: NoteStatus.Draft,
      }

      if (!editedNote.isSaved) {
        editedNote.isSaved = true

        const mapItem = state.map.items[action.payload.mapItemIndex]
        const mapItemNotes = getMapItemNotes(mapItem)

        const templateItemNumber =
          state.map.templateItems[action.payload.templateItemIndex].number

        let notes = mapItemNotes.get(templateItemNumber)

        if (!notes) {
          notes = {
            templateItemNumber,
            notes: [],
            noteGroups: [],
          }

          state.map.items[action.payload.mapItemIndex].notes.push(notes)
        }

        notes.notes.push(newNote)
      } else {
        const mapNote =
          state.map.items[action.payload.mapItemIndex].notes[
            action.payload.templateItemIndex
          ].notes[action.payload.noteIndex]

        mapNote.name = newNote.name
        mapNote.content = newNote.content
        mapNote.status = newNote.status
      }

      editedNote.isEditing = false

      state.isEditingNote = false
    },
    saveCreatedNote: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      const editedNote =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].notes[action.payload.noteIndex]

      const newNote = {
        name: editedNote.name,
        content: editedNote.content,
        status: NoteStatus.Created,
        createdDate: new Date().toISOString(),
      }

      if (!editedNote.isSaved) {
        editedNote.isSaved = true

        const mapItem = state.map.items[action.payload.mapItemIndex]
        const mapItemNotes = getMapItemNotes(mapItem)

        const templateItemNumber =
          state.map.templateItems[action.payload.templateItemIndex].number

        let notes = mapItemNotes.get(templateItemNumber)

        if (!notes) {
          notes = {
            templateItemNumber,
            notes: [],
            noteGroups: [],
          }

          state.map.items[action.payload.mapItemIndex].notes.push(notes)
        }

        notes.notes.push(newNote)
      } else {
        const mapNote =
          state.map.items[action.payload.mapItemIndex].notes[
            action.payload.templateItemIndex
          ].notes[action.payload.noteIndex]

        mapNote.name = newNote.name
        mapNote.content = newNote.content
        mapNote.status = newNote.status
        mapNote.createdDate = newNote.createdDate
      }

      editedNote.isEditing = false
      editedNote.createdDate = newNote.createdDate
      editedNote.status = newNote.status

      state.isEditingNote = false
    },
    saveEditedNote: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      const editedNote =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].notes[action.payload.noteIndex]

      const newNote = {
        name: editedNote.name,
        content: editedNote.content,
        status: NoteStatus.Edited,
        editedDate: new Date().toISOString(),
      }

      const mapNote =
        state.map.items[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].notes[action.payload.noteIndex]

      mapNote.name = newNote.name
      mapNote.content = newNote.content
      mapNote.status = newNote.status
      mapNote.editedDate = newNote.editedDate

      editedNote.isEditing = false
      editedNote.editedDate = mapNote.editedDate
      editedNote.status = mapNote.status

      state.isEditingNote = false
    },
    setNoteGroupName: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        name: string
      }>
    ) => {
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].name = action.payload.name
    },
    addNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      state.isEditingNote = true

      const noteGroup = {
        name: '',
        notes: [],
        isEditing: true,
      }

      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups.push(noteGroup)

      const mapItem = state.map.items[action.payload.mapItemIndex]
      const mapItemNotes = getMapItemNotes(mapItem)

      const templateItemNumber =
        state.map.templateItems[action.payload.templateItemIndex].number

      let notes = mapItemNotes.get(templateItemNumber)

      if (!notes) {
        notes = {
          templateItemNumber,
          notes: [],
          noteGroups: [],
        }

        state.map.items[action.payload.mapItemIndex].notes.push(notes)
      }

      notes.noteGroups.push(noteGroup)
    },
    removeNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      state.isEditingNote = true

      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups.splice(action.payload.noteGroupIndex, 1)

      const templateItemNumber =
        state.map.templateItems[action.payload.templateItemIndex].number

      const index = state.map.items[
        action.payload.mapItemIndex
      ].notes.findIndex(
        (item) => item.templateItemNumber === templateItemNumber
      )

      const mapItemNotes =
        state.map.items[action.payload.mapItemIndex].notes[index]

      mapItemNotes.noteGroups.splice(action.payload.noteGroupIndex, 1)

      if (
        mapItemNotes.noteGroups.length === 0 &&
        mapItemNotes.notes.length === 0
      ) {
        state.map.items[action.payload.mapItemIndex].notes.splice(index, 1)
      }

      state.isEditingNote = false
    },
    startEditNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
      }>
    ) => {
      state.isEditingNote = true
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].isEditing = true
    },
    saveNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      state.map.items[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].name =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].noteGroups[action.payload.noteGroupIndex].name

      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].isEditing = false
      state.isEditingNote = false
    },

    setNoteGroupNoteName: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
        name: string
      }>
    ) => {
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].notes[
        action.payload.noteIndex
      ].name = action.payload.name
    },
    setNoteGroupNoteContent: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
        content: string
      }>
    ) => {
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].notes[
        action.payload.noteIndex
      ].content = action.payload.content
    },
    addNoteToNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
      }>
    ) => {
      state.isEditingNote = true
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].notes.push({
        name: '',
        content: '',
        status: NoteStatus.Draft,
        isSaved: false,
        isEditing: true,
      })
    },
    removeNoteFromNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      state.isEditingNote = true

      const note =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].noteGroups[action.payload.noteGroupIndex].notes[
          action.payload.noteIndex
        ]

      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].notes.splice(
        action.payload.noteIndex,
        1
      )

      if (note.isSaved) {
        const templateItemNumber =
          state.map.templateItems[action.payload.templateItemIndex].number

        const index = state.map.items[
          action.payload.mapItemIndex
        ].notes.findIndex(
          (item) => item.templateItemNumber === templateItemNumber
        )

        state.map.items[action.payload.mapItemIndex].notes[index].noteGroups[
          action.payload.noteGroupIndex
        ].notes.splice(action.payload.noteIndex, 1)
      }

      state.isEditingNote = false
    },
    startEditNoteInNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
      }>
    ) => {
      state.isEditingNote = true
      state.mapItems[action.payload.mapItemIndex].notes[
        action.payload.templateItemIndex
      ].noteGroups[action.payload.noteGroupIndex].notes[
        action.payload.noteIndex
      ].isEditing = true
    },
    saveDraftNoteInNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      const editedNote =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].noteGroups[action.payload.noteGroupIndex].notes[
          action.payload.noteIndex
        ]

      const newNote = {
        name: editedNote.name,
        content: editedNote.content,
        status: NoteStatus.Draft,
      }

      if (!editedNote.isSaved) {
        editedNote.isSaved = true

        const templateItemNumber =
          state.map.templateItems[action.payload.templateItemIndex].number

        const index = state.map.items[
          action.payload.mapItemIndex
        ].notes.findIndex(
          (item) => item.templateItemNumber === templateItemNumber
        )

        state.map.items[action.payload.mapItemIndex].notes[index].noteGroups[
          action.payload.noteGroupIndex
        ].notes.push(newNote)
      } else {
        const mapNote =
          state.map.items[action.payload.mapItemIndex].notes[
            action.payload.templateItemIndex
          ].noteGroups[action.payload.noteGroupIndex].notes[
            action.payload.noteIndex
          ]

        mapNote.name = newNote.name
        mapNote.content = newNote.content
        mapNote.status = newNote.status
      }

      editedNote.isEditing = false

      state.isEditingNote = false
    },
    saveCreatedNoteInNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      const editedNote =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].noteGroups[action.payload.noteGroupIndex].notes[
          action.payload.noteIndex
        ]

      const newNote = {
        name: editedNote.name,
        content: editedNote.content,
        status: NoteStatus.Created,
        createdDate: new Date().toISOString(),
      }

      if (!editedNote.isSaved) {
        editedNote.isSaved = true

        const templateItemNumber =
          state.map.templateItems[action.payload.templateItemIndex].number

        const index = state.map.items[
          action.payload.mapItemIndex
        ].notes.findIndex(
          (item) => item.templateItemNumber === templateItemNumber
        )

        state.map.items[action.payload.mapItemIndex].notes[index].noteGroups[
          action.payload.noteGroupIndex
        ].notes.push(newNote)
      } else {
        const mapNote =
          state.map.items[action.payload.mapItemIndex].notes[
            action.payload.templateItemIndex
          ].noteGroups[action.payload.noteGroupIndex].notes[
            action.payload.noteIndex
          ]

        mapNote.name = newNote.name
        mapNote.content = newNote.content
        mapNote.status = newNote.status
        mapNote.createdDate = newNote.createdDate
      }

      editedNote.isEditing = false
      editedNote.createdDate = newNote.createdDate
      editedNote.status = newNote.status

      state.isEditingNote = false
    },
    saveEditedNoteInNoteGroup: (
      state,
      action: PayloadAction<{
        mapItemIndex: number
        templateItemIndex: number
        noteGroupIndex: number
        noteIndex: number
      }>
    ) => {
      if (!state.map) {
        return
      }

      const editedNote =
        state.mapItems[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].noteGroups[action.payload.noteGroupIndex].notes[
          action.payload.noteIndex
        ]

      const newNote = {
        name: editedNote.name,
        content: editedNote.content,
        status: NoteStatus.Edited,
        editedDate: new Date().toISOString(),
      }

      const mapNote =
        state.map.items[action.payload.mapItemIndex].notes[
          action.payload.templateItemIndex
        ].noteGroups[action.payload.noteGroupIndex].notes[
          action.payload.noteIndex
        ]

      mapNote.name = newNote.name
      mapNote.content = newNote.content
      mapNote.status = newNote.status
      mapNote.editedDate = newNote.editedDate

      editedNote.isEditing = false
      editedNote.editedDate = mapNote.editedDate
      editedNote.status = mapNote.status

      state.isEditingNote = false
    },
  },
})

export const {
  setMap,
  setResponseInfo,
  setMapItemsInfo,
  clearMapItemsInfo,
  addMapItem,
  setNoteName,
  setNoteContent,
  addNote,
  removeNote,
  startEditNote,
  saveDraftNote,
  saveCreatedNote,
  saveEditedNote,
  setNoteGroupName,
  addNoteGroup,
  removeNoteGroup,
  startEditNoteGroup,
  saveNoteGroup,
  setNoteGroupNoteName,
  setNoteGroupNoteContent,
  addNoteToNoteGroup,
  removeNoteFromNoteGroup,
  startEditNoteInNoteGroup,
  saveDraftNoteInNoteGroup,
  saveCreatedNoteInNoteGroup,
  saveEditedNoteInNoteGroup,
} = mapSlice.actions

export default mapSlice.reducer
