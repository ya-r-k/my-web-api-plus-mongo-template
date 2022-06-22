import './App.scss'
import { Routes, Route } from 'react-router-dom'
import HomePage from './commonPages/HomePage'
import PrivacyPage from './commonPages/PrivacyPage'
import NotFoundPage from './commonPages/NotFoundPage'
import DiariesPage from './diaryPages/DiariesPage'
import DiaryPage from './diaryPages/DiaryPage'
import MapsPage from './mapPages/MapsPage'
import MapPage from './mapPages/MapPage'

export default function App() {
  return (
    <Routes>
      <Route path='/' element={<HomePage />} />
      <Route path='/privacy' element={<PrivacyPage />} />
      <Route path='/diaries'>
        <Route index element={<DiariesPage />} />
        <Route path=':id'>
          <Route index element={<DiaryPage />} />
        </Route>
      </Route>
      <Route path='/maps'>
        <Route index element={<MapsPage />} />
        <Route path=':id'>
          <Route index element={<MapPage />} />
        </Route>
      </Route>
      <Route path='*' element={<NotFoundPage />} />
    </Routes>
  )
}
