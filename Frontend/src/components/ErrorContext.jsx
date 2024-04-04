import { createContext, useState } from 'react';
import PropTypes from 'prop-types';

export const ErrorContext = createContext();

export const ErrorProvider = ({ children }) => {
  const [errors, setErrors] = useState([]);
  const [showErrorModal, setshowErrorModal] = useState(false);

  function showError(errorsMessage) {
    //console.log(errorsMessage);
    setErrors(errorsMessage);
    setshowErrorModal(true);
  }

  function hideError() {
    setErrors([]);
    setshowErrorModal(false);
  }

  return (
    <ErrorContext.Provider
      value={{ errors, showErrorModal, showError, hideError }}
    >
      {children}
    </ErrorContext.Provider>
  );
};

ErrorProvider.propTypes = {
  children: PropTypes.node.isRequired,
};