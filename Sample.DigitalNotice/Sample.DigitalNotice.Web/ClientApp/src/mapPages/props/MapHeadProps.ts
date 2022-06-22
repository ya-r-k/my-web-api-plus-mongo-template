import { AnyAction, Dispatch } from '@reduxjs/toolkit'
import { Map } from '../../common/entities/Map'

export interface MapHeadProps {
  map: Map
  isEditing: boolean
  dispatch: Dispatch<AnyAction>
}
