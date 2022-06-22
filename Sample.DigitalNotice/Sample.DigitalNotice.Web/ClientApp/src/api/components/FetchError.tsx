import React from 'react'
import { FetchErrorProps as Props } from '../props/FetchErrorProps'

export default function FetchError({ error }: Props) {
  return <p>Error: {error.message}</p>
}
