import { Route, Routes } from 'react-router-dom'
import { RoutesNames } from './constants'
import 'bootstrap/dist/css/bootstrap.min.css'
import './App.css';
import NavBar from './components/NavBar'
import Home from './pages/Home'
import Locations from './pages/locations/Locations'
import Products from './pages/products/Products'
import Persons from './pages/persons/Persons'
import Login from './pages/Login'
import LocationsAdd from './pages/locations/LocationsAdd';

function App() {

  return (
    <>
      <NavBar />
      <Routes>
        <>
        <Route path={RoutesNames.LOGIN} element={<Login />} />
          <Route path={RoutesNames.HOME} element={<Home />} />

          <Route path={RoutesNames.LOCATIONS_LIST} element={<Locations />} />
          <Route path={RoutesNames.LOCATIONS_NEW} element={<LocationsAdd />} />

          <Route path={RoutesNames.PRODUCTS_LIST} element={<Products />} />
          <Route path={RoutesNames.PERSONS_LIST} element={<Persons />} />
        </>
      </Routes>
    </>
  )
}

export default App
