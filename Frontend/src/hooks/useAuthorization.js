import { useContext } from 'react';
import { AuthorizationContext } from '../components/AuthorizationContext';

export default function useAuth() {
  const context = useContext(AuthorizationContext);

  if (!context) {
    throw new Error('useAuthorization must be used inside ErrorProvider');
  }

  return context;
}