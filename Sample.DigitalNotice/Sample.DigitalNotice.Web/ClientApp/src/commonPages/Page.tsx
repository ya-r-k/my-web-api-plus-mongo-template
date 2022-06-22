import React from 'react'
import Header from '../header/Header'
import { PageProps as Props } from './props/PageProps'

export default function Page({ title, children }: Props) {
  React.useEffect(() => {
    document.title = `Digital Notice - ${title}`
  }, [title])

  return (
    <React.Fragment>
      <Header />
      <main>{children}</main>
    </React.Fragment>
  )
}
