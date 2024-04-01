import { Container } from "react-bootstrap";
import { useParams } from "react-router-dom";
import ProductService from "../../services/ProductService";
import { useEffect, useState } from "react";

export default function ProductsEdit() {
    const routeParams = useParams();
    const [product, setProduct] = useState({});
    const entityName = "product";

    async function fetchProduct() {
        await ProductService.getById(routeParams.id)
            .then((res) => {
                setProduct(res.data);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    useEffect(() => {
        fetchProduct();
    }, []);

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Details of product</h2>
                <p>{product.productName}</p>
                <p>{product.description}</p>
            </Container>
        </Container>
    );
}
