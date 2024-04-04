import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import WarehouseService from "../../services/WarehouseService";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";
import useError from "../../hooks/useError";

export default function Warehouses() {
    const [warehouses, setWarehouses] = useState();
    const navigate = useNavigate();
    const { showError } = useError();

    async function fetchWarehouses() {
        const response = await WarehouseService.get("Warehouse");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setWarehouses(response.data);
    }

    useEffect(() => {
        fetchWarehouses();
    }, []);

    async function removeWarehouse(id) {
        const response = await WarehouseService.remove("Warehouse", id);
        showError(response.data);
        if (response.ok) {
            fetchWarehouses();
        }
    }

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.WAREHOUSES_CREATE} entity={"warehouse"} />
            </Container>
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
        </Container>
    );
}
