export interface FetchBlockProps {
  url: string
  requestInfo: any
  Child: (data: any) => JSX.Element
}
