import { NoticeStatus } from '../enums/NoticeStatus'
import { NoticeType } from '../enums/NoticeType'
import { MapItem } from './MapItem'
import { TemplateItem } from './TemplateItem'

export type Map = {
  id?: string
  name: string
  description: string
  createdDate?: string
  status: NoticeStatus
  type: NoticeType
  templateItems: TemplateItem[]
  withNameColumn: boolean
  items: MapItem[]
}
