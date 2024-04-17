import React from "react";
import Pagination from "react-bootstrap/Pagination";

export default function MyPagination({ currentPage, totalPages, onPageChange }) {
    const handlePageChange = (page) => {
        onPageChange(page);
    };

    let items = [];
    for (let number = 1; number <= totalPages; number++) {
        items.push(
            <Pagination.Item
                key={number}
                active={number === currentPage}
                onClick={() => handlePageChange(number)}
            >
                {number}
            </Pagination.Item>
        );
    }

    return (
        <div>
            <Pagination>{items}</Pagination>
        </div>
    );
}
