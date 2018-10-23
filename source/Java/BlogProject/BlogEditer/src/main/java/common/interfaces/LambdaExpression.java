package common.interfaces;

public interface LambdaExpression<T, R> {
	R run(T node);
}
