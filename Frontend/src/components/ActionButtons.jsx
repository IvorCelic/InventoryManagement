import { Button, Col, Row } from "react-bootstrap";
import PropTypes from "prop-types";
import { Link } from "react-router-dom";

export default function ActionButtons({ cancel, action }) {
    return (
        <Row className="mb-0 flex-column flex-sm-row">
            <Col className="d-flex align-items-center mb-2 mb-sm-0">
                <Link className="btn btn-danger myButton" to={cancel}>
                    Cancel
                </Link>
            </Col>
            <Col className="d-flex align-items-center">
                <Button className="myButton" variant="primary" type="submit">
                    {action}
                </Button>
            </Col>
        </Row>
    );
}

ActionButtons.propTypes = {
    cancel: PropTypes.string.isRequired,
    action: PropTypes.string.isRequired,
};
