import java.util.Scanner;

public class DivisionCalculator {
    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);

        try {
            System.out.print("Enter the numerator : ");
            String numeratorInput = in.nextLine();
            int numerator = Integer.parseInt(numeratorInput);

            System.out.print("Enter denominator : ");
            String denominatorInput = in.nextLine();
            int denominator = Integer.parseInt(denominatorInput);


            double result = divide(numerator, denominator);

            System.out.println("Result : " + result);
        } catch (ArithmeticException e) {
            System.out.println("Error : Division by zero is not allowed .");
        } catch (NumberFormatException e) {
            System.out.println("Error : Invalid Input. please enter valid integer.");
        } catch (Exception e) {
            System.out.println("An Unexpected error occurred: " + e.getMessage());
        } finally {
            in.close();
            System.out.println("Calculation Complete");
        }
    }

    public static double divide(int numerator, int denominator) {
        if(denominator == 0) {
            throw new ArithmeticException("Division by zero");
        }
        return (double) numerator / denominator;
    }
}