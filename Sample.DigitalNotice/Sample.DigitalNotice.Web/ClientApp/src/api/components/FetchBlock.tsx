import FetchError from './FetchError'
import FetchLoading from './FetchLoading'
import { useRequest } from '../ts/use-request'
import { FetchBlockProps as Props } from '../props/FetchBlockProps'

export default function FetchComponent({ url, requestInfo, Child }: Props) {
  const { error, isLoaded, data } = useRequest(null, url, requestInfo)

  if (error) {
    return <FetchError {...{ error }} />
  } else if (!isLoaded) {
    return <FetchLoading />
  } else {
    return <Child {...{ data }} />
  }
}
