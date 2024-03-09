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
import LocationsCreate from './pages/locations/LocationsCreate';
import LocationsEdit from './pages/locations/LocationsEdit';

function App() {

  return (
    <>
      <NavBar />
      <Routes>
        <>
        <Route path={RoutesNames.LOGIN} element={<Login />} />
          <Route path={RoutesNames.HOME} element={<Home />} />

          <Route path={RoutesNames.LOCATIONS_LIST} element={<Locations />} />
          <Route path={RoutesNames.LOCATIONS_CREATE} element={<LocationsCreate />} />
          <Route path={RoutesNames.LOCATIONS_EDIT} element={<LocationsEdit />} />

          <Route path={RoutesNames.PRODUCTS_LIST} element={<Products />} />
          <Route path={RoutesNames.PERSONS_LIST} element={<Persons />} />
        </>
      </Routes>
    </>
  )
}

export default App
