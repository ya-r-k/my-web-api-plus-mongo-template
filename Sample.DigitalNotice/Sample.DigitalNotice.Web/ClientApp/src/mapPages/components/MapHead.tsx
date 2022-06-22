import React from 'react'
import { MapHeadProps as Props } from '../props/MapHeadProps'
import MapEditHead from './MapEditHead'
import MapInfoHead from './MapInfoHead'

export default function MapHead(props: Props) {
  return (
    <React.Fragment>
      {props.isEditing && <MapEditHead {...props} />}
      {!props.isEditing && <MapInfoHead {...props} />}
    </React.Fragment>
  )
}
