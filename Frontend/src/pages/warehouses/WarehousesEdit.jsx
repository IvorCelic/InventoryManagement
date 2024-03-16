import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import WarehouseService from "../../services/WarehouseService";
import { useEffect, useState } from "react";

export default function WarehousesEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [warehouse, setWarehouse] = useState({});
    const entityName = "warehouse";

    async function fetchWarehouse() {
        await WarehouseService.getById(routeParams.id)
            .then((res) => {
                setWarehouse(res.data);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    useEffect(() => {
        fetchWarehouse();
    }, []);

    async function editWarehouse(entityName) {
        const response = await WarehouseService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.WAREHOUSES_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target);

        const entityName = {
            warehouseName: data.get("warehousename"),
            description: data.get("description"),
        };

        editWarehouse(entityName);
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Edit warehouse</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="warehouseName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={warehouse.warehouseName}
                            name="warehousename"
                        />
                    </Form.Group>
                    <Form.Group controlId="description">
                        <Form.Label className="pt-4">Description</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={warehouse.description}
                            name="description"
                        />
                    </Form.Group>
                    <Row>
                        <Col>
                            <Link
                                className="btn btn-danger myButton"
                                to={RoutesNames.WAREHOUSES_LIST}
                            >
                                Cancel
                            </Link>
                        </Col>
                        <Col>
                            <Button
                                className="myButton"
                                variant="primary"
                                type="submit"
                            >
                                Save changes
                            </Button>
                        </Col>
                    </Row>
                </Form>
            </Container>
        </Container>
    );
}
