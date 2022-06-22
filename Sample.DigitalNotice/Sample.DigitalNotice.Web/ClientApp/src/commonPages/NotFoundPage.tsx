import React from 'react'
import Page from './Page'

export default function NotFoundPage() {
  return (
    <Page {...{ title: 'Not Found' }}>
      <h1>Page is not found!</h1>
    </Page>
  )
}
