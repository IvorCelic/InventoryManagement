import { createContext, useEffect, useState } from 'react';
import { AuthorizationService } from '../services/AuthorizationService';
import { useNavigate } from 'react-router-dom';
import { RoutesNames } from '../constants';
import useError from '../hooks/useError';
import useLoading from '../hooks/useLoading';

export const AuthContext = createContext();

export function AuthorizationProvider({ children }) {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [authToken, setAuthToken] = useState('');
  const { showLoading, hideLoading } = useLoading();

  const { prikaziError } = useError();
  const navigate = useNavigate();

  // useEffect hook na ovaj način koristimo da prilikom prve inicijalizacije
  // provjerimo postoji li bearer token u local storageu i ako postoji ,
  // automatski ulogiramo korisnika. Također ako bearer token ne postoji ,
  // u else dijelu štitimo aplikaciju tako da korisnik ne može pristupiti zaštićenim rutama
  useEffect(() => {
    const token = localStorage.getItem('Bearer');

    if (token) {
      setAuthToken(token);
      setIsLoggedIn(true);
    } else {
      navigate(RoutesNames.HOME);
    }
  }, []);

  async function login(userData) {
    showLoading();
    const response = await AuthorizationService(userData);
    hideLoading();
    if (response.ok) {
      localStorage.setItem('Bearer', response.data);
      setAuthToken(response.data);
      setIsLoggedIn(true);
      navigate(RoutesNames.NADZORNA_PLOCA);
    } else {
      console.log()
      prikaziError(response.data);
      localStorage.setItem('Bearer', '');
      setAuthToken('');
      setIsLoggedIn(false);
    }
  }

  function logout() {
    localStorage.setItem('Bearer', '');
    setAuthToken('');
    setIsLoggedIn(false);
    navigate(RoutesNames.HOME);
  }

  const value = {
    isLoggedIn,
    authToken,
    login,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}