import React from "react";
import { Link } from "react-router-dom";
import { GrFormAdd } from "react-icons/gr";
import { FaSearch } from "react-icons/fa";
import { Row, Col } from "react-bootstrap";

const SearchAndAdd = ({ RouteName, entity }) => {
    return (
        <Row className="mb-0 flex-column flex-sm-row"> {/* Add Bootstrap's mb-2 class for margin-bottom */}
            <Col lg="4" className="d-flex align-items-center mb-2 mb-sm-0">
                <FaSearch />
                <input
                    type="text"
                    className="border-0 border-bottom searchInput"
                    placeholder="Search"
                />
            </Col>
            <Col className="d-flex align-items-center">
                <Link to={RouteName} className="btn btn-primary addButton">
                    <GrFormAdd size={22} /> Add new {entity}
                </Link>
            </Col>
        </Row>
    );
};

export default SearchAndAdd;
