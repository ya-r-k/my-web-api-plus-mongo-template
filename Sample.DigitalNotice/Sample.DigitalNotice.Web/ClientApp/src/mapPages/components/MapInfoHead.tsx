import { NoticeStatus } from '../../common/enums/NoticeStatus'
import { MapHeadProps as Props } from '../props/MapHeadProps'
import { setResponseInfo } from '../redux/mapSlice'

export default function MapInfoHead({ map, dispatch }: Props) {
  const onSave = () => {
    fetch(`https://localhost:44488/api/maps/${map.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(map),
    }).then((response) => {
      dispatch(
        setResponseInfo({
          status: response.status,
          statusText: response.statusText,
        })
      )
    })
  }

  const onDelete = () => {
    fetch(`https://localhost:44488/api/maps/${map.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    }).then((response) => {
      if (response.status === 204) {
        window.location.href = '/maps'
      }
    })
  }

  return (
    <div {...{ className: 'map-head' }}>
      <h1>{map.name.length !== 0 ? map.name : 'No name'}</h1>
      <div {...{ className: 'actions-block' }}>
        <button {...{ className: 'submit', onClick: onSave }}>Save map</button>
        <button
          {...{
            className: 'edit',
          }}
        >
          Edit map common information
        </button>
        <button {...{ className: 'remove', onClick: onDelete }}>
          Delete map
        </button>
      </div>
      <p>{map.description.length > 0 ? map.description : 'No description'}</p>
      {map.templateItems.length > 0 && (
        <div>
          <h2>Template</h2>
          {map.templateItems.map((item) => {
            return (
              <div {...{ key: item.number, className: 'template-item-card' }}>
                <div {...{ className: 'title-block' }}>
                  <h3>{item.name}</h3>
                </div>
                <p>{item.description ? item.description : 'No description'}</p>
              </div>
            )
          })}
        </div>
      )}
      {map.status === NoticeStatus.Draft && <p>Draft</p>}
      {map.status === NoticeStatus.Created && map.createdDate && (
        <p>Created: {new Date(map.createdDate).toLocaleString()}</p>
      )}
    </div>
  )
}
