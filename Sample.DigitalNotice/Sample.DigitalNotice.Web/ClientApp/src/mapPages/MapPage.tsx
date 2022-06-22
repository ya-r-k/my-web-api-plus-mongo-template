import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useParams } from 'react-router-dom'
import { MapItemInfo } from '../common/entities/MapItemInfo'
import { NoteStatus } from '../common/enums/NoteStatus'
import NotFoundPage from '../commonPages/NotFoundPage'
import Page from '../commonPages/Page'
import { RootState } from '../store/store'
import MapHead from './components/MapHead'
import NoteCard from './components/NoteCard'
import { MapPageParams } from './params/MapPageParams'
import {
  addMapItem,
  addNote,
  addNoteGroup,
  addNoteToNoteGroup,
  clearMapItemsInfo,
  removeNoteFromNoteGroup,
  removeNoteGroup,
  saveCreatedNoteInNoteGroup,
  saveDraftNoteInNoteGroup,
  saveEditedNoteInNoteGroup,
  saveNoteGroup,
  setMap,
  setMapItemsInfo,
  setNoteGroupName,
  setNoteGroupNoteContent,
  setNoteGroupNoteName,
  setResponseInfo,
  startEditNoteGroup,
  startEditNoteInNoteGroup,
} from './redux/mapSlice'

export default function MapPage() {
  const data = useSelector((state: RootState) => state.mapReducer)
  const dispatch = useDispatch()
  const { id } = useParams<MapPageParams>()

  useEffect(() => {
    dispatch(clearMapItemsInfo())

    fetch(`https://localhost:44488/api/maps/${id}`).then((response) => {
      if (response.status === 200) {
        response.json().then((result) => dispatch(setMap(result)))
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
    if (data.mapItems.length === 0 && data.map) {
      const templateItemsNumbers = data.map.templateItems.map(
        (item) => item.number
      )

      const mapItems: MapItemInfo[] = data.map.items.map((item, index) => {
        const mapItemNotes = new Map(
          item.notes.map((item) => {
            return [item.templateItemNumber, item]
          })
        )

        return {
          ...item,
          notes: templateItemsNumbers.map((item) => {
            const notes = mapItemNotes.get(item)

            return {
              templateItemNumber: item,
              noteGroups:
                notes?.noteGroups.map((item) => {
                  return {
                    ...item,
                    isEditing: false,
                    notes: item.notes.map((item, index) => {
                      return { ...item, index, isSaved: true, isEditing: false }
                    }),
                  }
                }) || [],
              notes:
                notes?.notes.map((item, index) => {
                  return { ...item, index, isSaved: true, isEditing: false }
                }) || [],
            }
          }),
          index,
          isSaved: true,
          isEditing: false,
        }
      })

      dispatch(setMapItemsInfo(mapItems))
    }
  }, [data.map])

  return (
    <React.Fragment>
      {(data.responseInfo?.status === 404 ||
        data.responseInfo?.status === 400) && <NotFoundPage />}
      {data.map && (
        <Page {...{ title: 'Map' }}>
          <MapHead
            {...{ map: data.map, dispatch, isEditing: data.isEditing }}
          />
          <div {...{ className: 'map-content' }}>
            <h2>Content</h2>
            <table>
              <thead>
                <tr>
                  {data.map.withNameColumn && <th>Name</th>}
                  {data.map.templateItems.map((item) => {
                    return <th {...{ key: item.number }}>{item.name}</th>
                  })}
                </tr>
              </thead>
              <tbody>
                {data.mapItems.map((item, mapItemIndex) => {
                  return (
                    <tr {...{ key: mapItemIndex }}>
                      {data.map?.withNameColumn && <th>{item.name}</th>}
                      {item.notes.map((item, templateItemIndex) => {
                        return (
                          <td>
                            <div
                              {...{
                                key: item.templateItemNumber,
                                className: 'notes-list',
                              }}
                            >
                              {item.notes.length === 0 &&
                                item.noteGroups.length === 0 && (
                                  <div {...{ className: 'note-card' }}>
                                    <p>No notes</p>
                                  </div>
                                )}
                              {item.notes.map((item, noteIndex) => {
                                return (
                                  <NoteCard
                                    {...{
                                      key: noteIndex,
                                      noteInfo: item,
                                      dispatch,
                                      isEditingNote: data.isEditingNote,
                                      mapItemIndex,
                                      templateItemIndex,
                                      noteIndex,
                                    }}
                                  />
                                )
                              })}
                              {!data.isEditingNote && (
                                <button
                                  {...{
                                    className: 'add',
                                    onClick: () =>
                                      dispatch(
                                        addNote({
                                          mapItemIndex,
                                          templateItemIndex,
                                        })
                                      ),
                                  }}
                                >
                                  + Add note
                                </button>
                              )}
                              {item.noteGroups.map((item, noteGroupIndex) => {
                                return (
                                  <div
                                    {...{
                                      key: noteGroupIndex,
                                      className: 'note-group-block',
                                    }}
                                  >
                                    {item.isEditing && (
                                      <div {...{ className: 'title-block' }}>
                                        <input
                                          {...{
                                            value: item.name,
                                            placeholder: 'Note group name',
                                            onChange: (e) =>
                                              dispatch(
                                                setNoteGroupName({
                                                  mapItemIndex,
                                                  templateItemIndex,
                                                  noteGroupIndex,
                                                  name: e.target.value,
                                                })
                                              ),
                                          }}
                                        />
                                        <div
                                          {...{ className: 'actions-block' }}
                                        >
                                          <button
                                            {...{
                                              className: 'edit',
                                              onClick: () =>
                                                dispatch(
                                                  saveNoteGroup({
                                                    mapItemIndex,
                                                    templateItemIndex,
                                                    noteGroupIndex,
                                                  })
                                                ),
                                            }}
                                          >
                                            Save
                                          </button>
                                          <button
                                            {...{
                                              className: 'remove',
                                              onClick: () =>
                                                dispatch(
                                                  removeNoteGroup({
                                                    mapItemIndex,
                                                    templateItemIndex,
                                                    noteGroupIndex,
                                                  })
                                                ),
                                            }}
                                          >
                                            Delete
                                          </button>
                                        </div>
                                      </div>
                                    )}
                                    {!item.isEditing && (
                                      <div {...{ className: 'title-block' }}>
                                        <h3>
                                          {item.name.length !== 0
                                            ? item.name
                                            : 'No name'}
                                        </h3>
                                        {!data.isEditingNote && (
                                          <div
                                            {...{ className: 'actions-block' }}
                                          >
                                            <span>
                                              <button
                                                {...{
                                                  className: 'edit',
                                                  onClick: () =>
                                                    dispatch(
                                                      startEditNoteGroup({
                                                        mapItemIndex,
                                                        templateItemIndex,
                                                        noteGroupIndex,
                                                      })
                                                    ),
                                                }}
                                              >
                                                Edit
                                              </button>
                                            </span>
                                            <span>
                                              <button
                                                {...{
                                                  className: 'remove',
                                                  onClick: () =>
                                                    dispatch(
                                                      removeNoteGroup({
                                                        mapItemIndex,
                                                        templateItemIndex,
                                                        noteGroupIndex,
                                                      })
                                                    ),
                                                }}
                                              >
                                                Delete
                                              </button>
                                            </span>
                                          </div>
                                        )}
                                      </div>
                                    )}
                                    {item.notes.map((item, noteIndex) => {
                                      return (
                                        <React.Fragment>
                                          {item.isEditing && (
                                            <div
                                              {...{ className: 'note-card' }}
                                            >
                                              <div
                                                {...{
                                                  className: 'title-block',
                                                }}
                                              >
                                                <input
                                                  {...{
                                                    value: item.name,
                                                    placeholder: 'Note name',
                                                    onChange: (e) => {
                                                      dispatch(
                                                        setNoteGroupNoteName({
                                                          name: e.target.value,
                                                          mapItemIndex,
                                                          templateItemIndex,
                                                          noteGroupIndex,
                                                          noteIndex,
                                                        })
                                                      )
                                                    },
                                                  }}
                                                />
                                                <div
                                                  {...{
                                                    className: 'actions-block',
                                                  }}
                                                >
                                                  {item.status ===
                                                    NoteStatus.Draft && (
                                                    <button
                                                      {...{
                                                        className: 'submit',
                                                        onClick: () =>
                                                          dispatch(
                                                            saveDraftNoteInNoteGroup(
                                                              {
                                                                mapItemIndex,
                                                                templateItemIndex,
                                                                noteGroupIndex,
                                                                noteIndex,
                                                              }
                                                            )
                                                          ),
                                                      }}
                                                    >
                                                      Save note as draft
                                                    </button>
                                                  )}
                                                  {item.status ===
                                                    NoteStatus.Draft && (
                                                    <button
                                                      {...{
                                                        className: 'add',
                                                        onClick: () =>
                                                          dispatch(
                                                            saveCreatedNoteInNoteGroup(
                                                              {
                                                                mapItemIndex,
                                                                templateItemIndex,
                                                                noteGroupIndex,
                                                                noteIndex,
                                                              }
                                                            )
                                                          ),
                                                      }}
                                                    >
                                                      Save note
                                                    </button>
                                                  )}
                                                  {(item.status ===
                                                    NoteStatus.Created ||
                                                    item.status ===
                                                      NoteStatus.Edited) && (
                                                    <button
                                                      {...{
                                                        className: 'edit',
                                                        onClick: () =>
                                                          dispatch(
                                                            saveEditedNoteInNoteGroup(
                                                              {
                                                                mapItemIndex,
                                                                templateItemIndex,
                                                                noteGroupIndex,
                                                                noteIndex,
                                                              }
                                                            )
                                                          ),
                                                      }}
                                                    >
                                                      Save note
                                                    </button>
                                                  )}
                                                  <button
                                                    {...{
                                                      className: 'remove',
                                                      onClick: () =>
                                                        dispatch(
                                                          removeNoteFromNoteGroup(
                                                            {
                                                              mapItemIndex,
                                                              templateItemIndex,
                                                              noteGroupIndex,
                                                              noteIndex,
                                                            }
                                                          )
                                                        ),
                                                    }}
                                                  >
                                                    Delete note
                                                  </button>
                                                </div>
                                              </div>
                                              <textarea
                                                {...{
                                                  value: item.content,
                                                  placeholder: 'Note content',
                                                  onChange: (e) => {
                                                    dispatch(
                                                      setNoteGroupNoteContent({
                                                        content: e.target.value,
                                                        mapItemIndex,
                                                        templateItemIndex,
                                                        noteGroupIndex,
                                                        noteIndex,
                                                      })
                                                    )
                                                  },
                                                }}
                                              />
                                            </div>
                                          )}
                                          {!item.isEditing && (
                                            <div
                                              {...{ className: 'note-card' }}
                                            >
                                              <div
                                                {...{
                                                  className: 'title-block',
                                                }}
                                              >
                                                <h3>{item.name}</h3>
                                                {!data.isEditingNote && (
                                                  <div
                                                    {...{
                                                      className:
                                                        'actions-block',
                                                    }}
                                                  >
                                                    <button
                                                      {...{
                                                        className: 'edit',
                                                        onClick: () =>
                                                          dispatch(
                                                            startEditNoteInNoteGroup(
                                                              {
                                                                mapItemIndex,
                                                                templateItemIndex,
                                                                noteGroupIndex,
                                                                noteIndex,
                                                              }
                                                            )
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
                                                            removeNoteFromNoteGroup(
                                                              {
                                                                mapItemIndex,
                                                                templateItemIndex,
                                                                noteGroupIndex,
                                                                noteIndex,
                                                              }
                                                            )
                                                          ),
                                                      }}
                                                    >
                                                      Delete note
                                                    </button>
                                                  </div>
                                                )}
                                              </div>
                                              <p>{item.content}</p>
                                              {item.status ===
                                                NoteStatus.Draft && (
                                                <p>Draft</p>
                                              )}
                                              {item.status ===
                                                NoteStatus.Created &&
                                                item.createdDate && (
                                                  <p>
                                                    Created:{' '}
                                                    {new Date(
                                                      item.createdDate
                                                    ).toLocaleString()}
                                                  </p>
                                                )}
                                              {item.status ===
                                                NoteStatus.Edited &&
                                                item.editedDate && (
                                                  <p>
                                                    Edited:{' '}
                                                    {new Date(
                                                      item.editedDate
                                                    ).toLocaleString()}
                                                  </p>
                                                )}
                                            </div>
                                          )}
                                        </React.Fragment>
                                      )
                                    })}
                                    {!data.isEditingNote && (
                                      <button
                                        {...{
                                          className: 'add',
                                          onClick: () =>
                                            dispatch(
                                              addNoteToNoteGroup({
                                                mapItemIndex,
                                                templateItemIndex,
                                                noteGroupIndex,
                                              })
                                            ),
                                        }}
                                      >
                                        + Add note
                                      </button>
                                    )}
                                  </div>
                                )
                              })}
                              {!data.isEditingNote && (
                                <button
                                  {...{
                                    className: 'add',
                                    onClick: () =>
                                      dispatch(
                                        addNoteGroup({
                                          mapItemIndex,
                                          templateItemIndex,
                                        })
                                      ),
                                  }}
                                >
                                  + Add note group
                                </button>
                              )}
                            </div>
                          </td>
                        )
                      })}
                    </tr>
                  )
                })}
              </tbody>
            </table>
            {!data.isEditingNote && (
              <button
                {...{ className: 'add', onClick: () => dispatch(addMapItem()) }}
              >
                + Add map item
              </button>
            )}
          </div>
        </Page>
      )}
    </React.Fragment>
  )
}
