import { NoticeStatus } from '../../common/enums/NoticeStatus'
import { DiaryHeadProps as Props } from '../props/DiaryHeadProps'
import {
  saveCreatedDiary,
  saveDraftDiary,
  setDiaryDescription,
  setDiaryName,
} from '../redux/diarySlice'

export default function DiaryEditHead({ diary, dispatch }: Props) {
  return (
    <div {...{ className: 'diary-head' }}>
      <h1>{diary.name.length !== 0 ? diary.name : 'No name'}</h1>
      <div {...{ className: 'actions-block' }}>
        {diary.status === NoticeStatus.Draft && (
          <button
            {...{
              className: 'submit',
              onClick: () => {
                dispatch(saveDraftDiary())
              },
            }}
          >
            Save diary as draft
          </button>
        )}
        {diary.status === NoticeStatus.Draft && (
          <button
            {...{
              className: 'add',
              onClick: () => {
                dispatch(saveCreatedDiary())
              },
            }}
          >
            Save diary
          </button>
        )}
        {diary.status === NoticeStatus.Created && (
          <button {...{ className: 'edit' }}>Save diary</button>
        )}
      </div>
      <input
        {...{
          placeholder: 'Diary name',
          value: diary.name,
          onChange: (e) => dispatch(setDiaryName({ name: e.target.value })),
        }}
      />
      <textarea
        {...{
          placeholder: 'Diary description',
          value: diary.description,
          onChange: (e) =>
            dispatch(setDiaryDescription({ description: e.target.value })),
        }}
      />
    </div>
  )
}
