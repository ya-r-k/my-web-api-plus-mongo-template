import { NoticeStatus } from '../../common/enums/NoticeStatus'
import { DiaryHeadProps as Props } from '../props/DiaryHeadProps'
import { setResponseInfo, startEditDiaryCommonInfo } from '../redux/diarySlice'

export default function DiaryInfoHead({ diary, dispatch }: Props) {
  const onSave = () => {
    fetch(`https://localhost:44488/api/diaries/${diary.id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(diary),
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
    fetch(`https://localhost:44488/api/diaries/${diary.id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(diary),
    }).then((response) => {
      if (response.status === 204) {
        window.location.href = '/diaries'
      }
    })
  }

  return (
    <div {...{ className: 'diary-head' }}>
      <h1>{diary.name.length !== 0 ? diary.name : 'No name'}</h1>
      <div {...{ className: 'actions-block' }}>
        <button {...{ className: 'submit', onClick: onSave }}>
          Save diary
        </button>
        <button
          {...{
            className: 'edit',
            onClick: () => dispatch(startEditDiaryCommonInfo()),
          }}
        >
          Edit diary common information
        </button>
        <button {...{ className: 'remove', onClick: onDelete }}>
          Delete diary
        </button>
      </div>
      <p>
        {diary.description.length !== 0 ? diary.description : 'No description'}
      </p>
      {diary.status === NoticeStatus.Draft && <p>Draft</p>}
      {diary.status === NoticeStatus.Created && diary.createdDate && (
        <p>Created: {new Date(diary.createdDate).toLocaleString()}</p>
      )}
    </div>
  )
}
