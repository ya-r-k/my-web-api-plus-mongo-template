import { NoticeStatus } from '../../common/enums/NoticeStatus'
import { MapHeadProps as Props } from '../props/MapHeadProps'
import {} from '../redux/mapSlice'

export default function MapEditHead({ map, dispatch }: Props) {
  return (
    <div {...{ className: 'map-head' }}>
      <h1>{map.name.length !== 0 ? map.name : 'No name'}</h1>
      <div {...{ className: 'actions-block' }}>
        {map.status === NoticeStatus.Draft && (
          <button
            {...{
              className: 'submit',
            }}
          >
            Save map as draft
          </button>
        )}
        {map.status === NoticeStatus.Draft && (
          <button {...{ className: 'add' }}>Save map</button>
        )}
        {map.status === NoticeStatus.Created && (
          <button {...{ className: 'edit' }}>Save map</button>
        )}
      </div>
      <input {...{ placeholder: 'Map name', value: map.name }} />
      <textarea
        {...{ placeholder: 'Map description', value: map.description }}
      />
    </div>
  )
}
