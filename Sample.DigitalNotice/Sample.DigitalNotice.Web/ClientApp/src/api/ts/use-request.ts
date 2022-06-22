import React from 'react'

export function useRequest(initialModel: any, url: string, requestInfo: any) {
  const [error, setError] = React.useState()
  const [isLoaded, setIsLoaded] = React.useState(false)
  const [data, setData] = React.useState(initialModel)

  React.useEffect(() => {
    fetch(url, requestInfo)
      .then(
        (result) => result.json(),
        (error) => {
          setIsLoaded(true)
          setError(error)
        }
      )
      .then(
        (result) => {
          setIsLoaded(true)
          setData(result)
        },
        (error) => {
          setIsLoaded(true)
          setError(error)
        }
      )
  }, [url, requestInfo])

  return { error, isLoaded, data, setData }
}
