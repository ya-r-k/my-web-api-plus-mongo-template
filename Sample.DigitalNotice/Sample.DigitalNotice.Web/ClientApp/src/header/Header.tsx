import { Link } from 'react-router-dom'

const menuList = [
  { text: 'Home', link: '/' },
  { text: 'Privacy', link: '/privacy' },
  { text: 'Diaries', link: '/diaries' },
  { text: 'Maps', link: '/maps' },
]

export default function Header() {
  return (
    <header {...{ className: 'header' }}>
      <nav>
        {menuList.map((item, index) => {
          return <Link {...{ key: index, to: item.link }}>{item.text}</Link>
        })}
      </nav>
    </header>
  )
}
