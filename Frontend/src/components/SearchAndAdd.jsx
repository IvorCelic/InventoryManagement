import React, { useState } from "react";
import { Link } from "react-router-dom";
import { GrFormAdd } from "react-icons/gr";
import { FaSearch } from "react-icons/fa";
import { Row, Col } from "react-bootstrap";

export default function SearchAndAdd({ RouteName, entity, onSearch }) {
    const [searchValue, setSearchValue] = useState("");

    const handleSearchChange = (event) => {
        const { value } = event.target;
        setSearchValue(value);
    };

    const handleKeyDown = (event) => {
        if (event.key === "Enter") {
            onSearch(searchValue);
        }
    };

    return (
        <Col>
            <Row className="mb-0 flex-column flex-sm-row">
                <Col lg="4" className="d-flex align-items-center mb-2 mb-sm-0">
                    <FaSearch size={20} />
                    <input
                        type="text"
                        className="border-0 border-bottom searchInput"
                        placeholder="Search [Enter]"
                        value={searchValue}
                        onChange={handleSearchChange}
                        onKeyDown={handleKeyDown}
                    />
                </Col>
                <Col className="d-flex align-items-center">
                    <Link to={RouteName} className="btn btn-primary addButton">
                        <GrFormAdd size={22} /> Add new {entity}
                    </Link>
                </Col>
            </Row>
        </Col>
    );
}
