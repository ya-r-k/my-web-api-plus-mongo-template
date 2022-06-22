import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Map } from '../common/entities/Map'
import { NoticeStatus } from '../common/enums/NoticeStatus'
import { NoticeType } from '../common/enums/NoticeType'
import Page from '../commonPages/Page'

export default function MapsPage() {
  const [maps, setMaps] = useState<Map[]>([])

  useEffect(() => {
    fetch('https://localhost:44488/api/maps').then((response) => {
      if (response.status === 200) {
        response.json().then((result) => setMaps(result))
      }
    })
  }, [])

  const onCreateMap = () => {
    fetch('https://localhost:44488/api/maps', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        name: '',
        description: '',
        status: NoticeStatus.Draft,
        type: NoticeType.Map,
        withNameColumn: false,
        items: [],
        templateItems: [],
      } as Map),
    }).then((response) => {
      if (response.status === 201) {
        response
          .json()
          .then((result) => (window.location.href = `/maps/${result.id}`))
      }
    })
  }

  return (
    <Page {...{ title: 'Maps' }}>
      <h1>Maps</h1>
      <div>
        <button {...{ className: 'add', onClick: onCreateMap }}>New map</button>
        {maps.length > 0 &&
          maps.map((item) => {
            return (
              <div {...{ className: 'map-card', key: item.id }}>
                <Link {...{ to: `/maps/${item.id}` }}>
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
        {maps.length === 0 && (
          <div {...{ className: 'map-card' }}>
            <p>No any maps</p>
          </div>
        )}
      </div>
    </Page>
  )
}
