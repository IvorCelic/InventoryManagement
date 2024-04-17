import { useEffect, useState } from "react";
import { Button, Col, Container, Row, Table } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import WarehouseService from "../../services/WarehouseService";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";
import useError from "../../hooks/useError";
import MyPagination from "../../components/MyPagination";

export default function Warehouses() {
    const navigate = useNavigate();
    const { showError } = useError();

    const [warehouses, setWarehouses] = useState();
    const [totalWarehouses, setTotalWarehouses] = useState();
    const [page, setPage] = useState(1);
    const [condition, setCondition] = useState("");

    async function fetchWarehouses() {
        const responsePagination = await WarehouseService.getPagination(page, condition);
        const responseWarehouse = await WarehouseService.get("Warehouse");
        if (!responsePagination.ok) {
            showError(responsePagination.data);
            return;
        }

        if (responsePagination.data.length == 0) {
            setPage(page - 1);
            return;
        }
        setWarehouses(responsePagination.data);
        setTotalWarehouses(responseWarehouse.data.length);
    }

    useEffect(() => {
        fetchWarehouses();
    }, [page, condition]);

    async function removeWarehouse(id) {
        const response = await WarehouseService.remove("Warehouse", id);
        showError(response.data);
        if (response.ok) {
            fetchWarehouses();
        }
    }

    const totalPages = Math.ceil(totalWarehouses / 8);

    function handlePageChange(page) {
        setPage(page);
    }

    function handleSearch(searchTerm) {
        setPage(1);
        setCondition(searchTerm);
    }

    return (
        <Container>
            <Col>
                <Row>
                    <SearchAndAdd
                        RouteName={RoutesNames.WAREHOUSES_CREATE}
                        entity={"warehouse"}
                        onSearch={handleSearch}
                    />
                </Row>
                <Row>
                    <Table striped bordered hover responsive>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {warehouses &&
                                warehouses.map((warehouse, index) => (
                                    <tr key={index}>
                                        <td>{warehouse.warehouseName}</td>
                                        <td>{warehouse.description}</td>
                                        <td>
                                            <Container className="d-flex justify-content-center">
                                                <Button
                                                    variant="link"
                                                    className="me-2 actionButton"
                                                    onClick={() => {
                                                        navigate(`/warehouses/${warehouse.id}`);
                                                    }}
                                                >
                                                    <FaEdit size={25} />
                                                </Button>
                                                <Button
                                                    variant="link"
                                                    className="link-danger actionButton"
                                                    onClick={() => removeWarehouse(warehouse.id)}
                                                >
                                                    <FaTrash size={25} />
                                                </Button>
                                            </Container>
                                        </td>
                                    </tr>
                                ))}
                        </tbody>
                    </Table>
                </Row>
                <Row>
                    <MyPagination
                        currentPage={page}
                        totalPages={totalPages}
                        onPageChange={handlePageChange}
                    />
                </Row>
            </Col>
        </Container>
    );
}
