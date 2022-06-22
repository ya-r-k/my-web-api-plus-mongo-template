import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Diary } from '../common/entities/Diary'
import { NoticeStatus } from '../common/enums/NoticeStatus'
import { NoticeType } from '../common/enums/NoticeType'
import Page from '../commonPages/Page'

export default function DiariesPage() {
  const [diaries, setDiaries] = useState<Diary[]>([])

  useEffect(() => {
    fetch('https://localhost:44488/api/diaries').then((response) => {
      if (response.status === 200) {
        response.json().then((result) => setDiaries(result))
      }
    })
  }, [])

  const onCreateDiary = () => {
    fetch('https://localhost:44488/api/diaries', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        name: '',
        description: '',
        status: NoticeStatus.Draft,
        type: NoticeType.Diary,
        notes: [],
      } as Diary),
    }).then((response) => {
      if (response.status === 201) {
        response
          .json()
          .then((result) => (window.location.href = `/diaries/${result.id}`))
      }
    })
  }

  return (
    <Page {...{ title: 'Diaries' }}>
      <h1>Diaries</h1>
      <div>
        <button {...{ className: 'add', onClick: onCreateDiary }}>
          New diary
        </button>
        {diaries.length > 0 &&
          diaries.map((item) => {
            return (
              <div {...{ className: 'diary-card', key: item.id }}>
                <Link {...{ to: `/diaries/${item.id}` }}>
                  <b>{item.name.length !== 0 ? item.name : 'No name'}</b>
                </Link>
                <p>
                  {item.description.length !== 0
                    ? item.description
                    : 'No description'}
                </p>
                {item.status === NoticeStatus.Draft && <p>Draft</p>}
                {item.status === NoticeStatus.Created && item.createdDate && (
                  <p>Created: {new Date(item.createdDate).toLocaleString()}</p>
                )}
              </div>
            )
          })}
        {diaries.length === 0 && (
          <div {...{ className: 'diary-card' }}>
            <p>No any diaries</p>
          </div>
        )}
      </div>
    </Page>
  )
}
