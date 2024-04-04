import { Modal, Button } from "react-bootstrap";
import PropTypes from "prop-types";

export default function ErrorModal({ show, onHide, errors }) {
    console.log("Errors in modal: ", errors);
    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>Ups!</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <ul>
                    {errors && errors.map((error, index) => <li key={index}>{error.message}</li>)}
                </ul>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onHide}>
                    Zatvori
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

ErrorModal.propTypes = {
    errors: PropTypes.array,
    onHide: PropTypes.func.isRequired,
    show: PropTypes.bool.isRequired,
};
