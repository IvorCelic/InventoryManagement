import React, { useState } from "react";
import { Link } from "react-router-dom";
import { GrFormAdd } from "react-icons/gr";
import { FaSearch } from "react-icons/fa";
import { Row, Col, Button } from "react-bootstrap";

export default function SearchAndAdd({ RouteName, entity, onSearch }) {
    const [searchValue, setSearchValue] = useState("");

    const handleSearchChange = (event) => {
        const { value } = event.target;
        setSearchValue(value);
    };

    const handleSearch = () => {
        onSearch(searchValue);
    };

    return (
        <Row className="mb-0 flex-column flex-sm-row">
            <Col lg="4" className="d-flex align-items-center mb-2 mb-sm-0">
                <FaSearch />
                <input
                    type="text"
                    className="border-0 border-bottom searchInput"
                    placeholder="Search"
                    value={searchValue}
                    onChange={handleSearchChange}
                />
                <Button
                    variant="primary"
                    className="btn btn-primary addButton"
                    onClick={handleSearch}
                >
                    Search
                </Button>
            </Col>
            <Col className="d-flex align-items-center">
                <Link to={RouteName} className="btn btn-primary addButton">
                    <GrFormAdd size={22} /> Add new {entity}
                </Link>
            </Col>
        </Row>
    );
}
