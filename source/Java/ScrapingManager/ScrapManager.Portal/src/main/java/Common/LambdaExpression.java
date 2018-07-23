package Common;

public interface LambdaExpression<T, R> {
	R run(T node);
}
