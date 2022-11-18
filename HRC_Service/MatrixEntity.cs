using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace HRC_Service
{

    /// <summary>
    /// 
    /// Matrix class of type int
    /// 
    /// </summary>
    class MatrixEntity
    {
        #region Private MatrixEntity fields

        //Array for internal storage of elements.
        private int[,] _Mat;

        //Internal dimensions of a matrix, actually the number of rows and cols
        private int _row, _col;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a matrix of doubles
        /// </summary>
        /// <param name="Row">number of rows</param>
        /// <param name="Col">number of columns</param>
        public MatrixEntity(int Row, int Col)
        {
            //create matrix
            this._row = Row;
            this._col = Col;
            _Mat = new int[Row, Col];
        }

        /// <summary>
        /// Construct a matrix of doubles, filled with initial value d
        /// </summary>
        /// <param name="Row">number of rows</param>
        /// <param name="Col">number of columns</param>
        /// <param name="d">initial double value</param>
        public MatrixEntity(int Row, int Col, int d)
        {
            this._row = Row;
            this._col = Col;
            _Mat = new int[Row, Col];
            for (int r = 0; r < _row; r++)
            {
                for (int c = 0; c < _col; c++)
                {
                    _Mat[r, c] = d;
                }
            }
        }

        /// <summary>
        /// Construct matrix from an array of doubles
        /// </summary>
        /// <param name="Ar">array of doubles</param>
        public MatrixEntity(int[,] Ar)
        {
            //create a matrix from an array of double
            this._row = Ar.GetUpperBound(0) + 1;    //zero based so plus one to get number of rows
            this._col = Ar.GetUpperBound(1) + 1;
            this._Mat = Ar;
        }

        #endregion

        #region	 Helper Methods

        /// <summary>
        /// Test if dimensions of a matrix and this one are equal
        /// </summary>
        /// <param name="R">matrix to test </param>
        /// <returns>true if dimensions of both matrices agree</returns>
        private bool CheckMatrixDimensions(MatrixEntity R)
        {
            if (R._row == this._row && R._col == this._col)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// for determinant calculation
        /// </summary>
        /// <param name="i">row</param>
        /// <param name="j">column</param>
        /// <returns></returns>
        private static int SignOfElement(int i, int j)
        {
            int sign = ((i + j) % 2 == 0) ? 1 : -1;
            return sign;
        }

        /// <summary>
        /// This method determines the sub matrix corresponding to a given element
        /// for determinant calculation
        /// </summary>
        /// <param name="input">the original matrix</param>
        /// <param name="i">the row</param>
        /// <param name="j">the column</param>
        /// <returns></returns>
        private static MatrixEntity CreateSmalleMatrixEntity(MatrixEntity input, int i, int j)
        {
            MatrixEntity output;
            if (!input.IsSquare()) throw new Exception("Matrix must be square.");
            else
            {
                output = new MatrixEntity(input._row - 1, input._col - 1);
                int x = 0, y = 0;
                for (int m = 0; m < input._row; m++, x++) //or _col
                {
                    if (m != i)
                    {
                        y = 0;
                        for (int n = 0; n < input._row; n++) //or _col
                        {
                            if (n != j)
                            {
                                output[x, y] = input[m, n];
                                y++;
                            }
                        }
                    }
                    else
                    {
                        x--;
                    }
                }
            }
            return output;
        }

        #endregion

        #region Properties

        /// <summary>
        /// index operator, to retrieve and set values in the matrix
        /// </summary>
        /// <param name="Row">zero index</param>
        /// <param name="Col">zero index</param>
        /// <returns>gets or sets element at Row and Col</returns>
        public int this[int Row, int Col]
        {
            get { return _Mat[Row, Col]; }
            set { _Mat[Row, Col] = value; }
        }

        /// <summary>
        /// return or adjust this matrix's rows
        /// </summary>
        public int Rows
        {
            get { return _row; }
        }

        /// <summary>
        /// return or adjust this matrix's columns
        /// </summary>
        public int Cols
        {
            get { return _col; }
        }

        /// <summary>
        /// Treat this matrix as an array
        /// </summary>
        public int[,] array
        {
            get { return _Mat; }
            set
            {
                this._row = value.GetUpperBound(0) + 1;
                this._col = value.GetUpperBound(1) + 1;
                _Mat = value;
            }
        }

        #endregion

        #region Handy methods

        /// <summary>
        /// Determine if I am squared
        /// </summary>
        /// <returns>true or false, depending</returns>
        public bool IsSquare()
        {
            return _col == _row;
        }

        /// <summary>
        /// Determine if I am symmetric
        /// </summary>
        /// <returns>true or false, depending</returns>
        public bool IsSymetric()
        {
            MatrixEntity M = new MatrixEntity(_Mat);
            return _Mat.Equals(M.Transpose());
        }

        /// <summary>
        /// Possible small rounding error
        /// </summary>
        /// <returns>true or false, depending</returns>
        public bool IsSingular()
        {
            return Determinant(this) == 0.0;
        }

        /// <summary>
        /// return identity matrix
        /// </summary>
        /// <param name="r">the rows</param>
        /// <param name="c">the columns</param>
        /// <returns>the identity matrix</returns>
        public static MatrixEntity Identity(int r, int c)
        {
            MatrixEntity MA = new MatrixEntity(r, c);
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    MA[i, j] = (i == j ? 1 : 0);
                }
            }
            return MA;
        }

        /// <summary>
        /// generates the zero matrix
        /// </summary>
        /// <param name="r">the rows</param>
        /// <param name="c">the columns</param>
        /// <returns>the zero matrix</returns>
        public static MatrixEntity Zero(int r, int c)
        {
            MatrixEntity MA = new MatrixEntity(r, c);
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    MA[i, j] = 0;
                }
            }
            return MA;
        }

        /// <summary>
        /// Generate a matrix filled with random doubles between 0 and 0.99999...
        /// </summary>
        /// <param name="r">the rows</param>
        /// <param name="c">the columns</param>
        /// <param name="seed"> a random seed value</param>
        /// <returns></returns>
        public static MatrixEntity Random(int r, int c, int seed)
        {
            Random random = new Random(seed);
            MatrixEntity R = new MatrixEntity(r, c);
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    R[i, j] = random.Next();
                }
            }
            return R;
        }

        /// <summary>
        /// generates a random matrix with integer numbers between - and + dispersion
        /// </summary>
        /// <param name="r">the number of rows</param>
        /// <param name="c">the number of columns</param>
        /// <param name="dispersion">the "span" of the numbers generated</param>
        /// <returns></returns>
        public static MatrixEntity RandomDisperse(int r, int c, int dispersion)
        {
            Random random = new Random();
            MatrixEntity matrix = new MatrixEntity(r, c);
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    matrix[i, j] = random.Next(-dispersion, dispersion);
                }
            }
            return matrix;
        }

        /// <summary>
        /// For a square matrix, the trace is the sum of the diagonal elements.
        /// See also Jacobi's formula
        /// </summary>
        /// <returns></returns>
        public double Trace()
        {
            double d = 0.0;
            for (int i = 0; i < Rows; i++)
            {
                if (i < Cols)
                    d += _Mat[i, i];
            }
            return d;
        }

        /// <summary>
        /// Transpose: change rows and columns
        /// </summary>
        /// <returns>the transposed matrix</returns>
        public MatrixEntity Transpose()
        {
            MatrixEntity Trans = new MatrixEntity(this._col, this._row);

            for (int i = 0; i < this._row; i++)
            {
                for (int j = 0; j < this._col; j++)
                {
                    Trans[j, i] = this[i, j];
                }
            }
            return Trans;
        }

        /// <summary>
        /// multiply this matrix by matrix M
        /// used in the * operator overload
        /// </summary>
        /// <param name="M"></param>
        /// <returns>multiplication</returns>
        public virtual MatrixEntity Multiply(MatrixEntity M)
        {
            MatrixEntity X = new MatrixEntity(this._row, M._col);
            for (int i = 0; i < this._row; i++)
            {
                for (int j = 0; j < M._col; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < this._col; k++) //this._col or M._row
                    {
                        sum += this[i, k] * M[k, j];
                    }
                    X[i, j] = sum;
                }
            }
            return X;
        }

        public override bool Equals(object obj)
        {
            return (obj is MatrixEntity) && this.Equals((MatrixEntity)obj);
        }

        public bool Equals(MatrixEntity m)
        {
            return _Mat == m._Mat;
        }

        public override int GetHashCode()
        {
            return _Mat.GetHashCode();
        }

        public static bool operator ==(MatrixEntity m1, MatrixEntity m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator !=(MatrixEntity m1, MatrixEntity m2)
        {
            return !m1.Equals(m2);
        }

        ///// <summary>
        ///// Get one row as a double array
        ///// </summary>
        ///// <param name="r"></param>
        ///// <returns></returns>
        //public RealVector GetRow(int r)
        //{
        //    if (r < 0 || r > _row)
        //    {
        //        throw new ArgumentOutOfRangeException("Row", r, " Row is out off range");
        //    }
        //    RealVector vec = new RealVector(this.Cols);
        //    for (int i = 0; i < Cols; i++)
        //    {
        //        vec[i] = _Mat[r, i];
        //    }
        //    return vec;
        //}

        ///// <summary>
        ///// Get one column as a double array
        ///// </summary>
        ///// <param name="c"></param>
        ///// <returns></returns>
        //public RealVector GetCol(int c)
        //{
        //    if (c < 0 || c > _col)
        //    {
        //        throw new ArgumentOutOfRangeException("Col", c, " Col is out off range");
        //    }
        //    RealVector vec = new RealVector(this.Rows);
        //    for (int i = 0; i < Rows; i++)
        //    {
        //        vec[i] = _Mat[i, c];
        //    }
        //    return vec;
        //}

        /// <summary>
        /// Swap rows in this matrix
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        public void SwapRows(int r1, int r2)
        {
            int temp = 0;
            for (int i = 0; i < Cols; i++)
            {
                temp = _Mat[r1, i];
                _Mat[r1, i] = _Mat[r2, i];
                _Mat[r2, i] = temp;
            }
        }

        /// <summary>
        /// Swap columns in this matrix
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        public void SwapCols(int c1, int c2)
        {
            int temp = 0;
            for (int i = 0; i < Rows; i++)
            {
                temp = _Mat[i, c1];
                _Mat[i, c1] = _Mat[i, c2];
                _Mat[i, c2] = temp;
            }
        }

        /// <summary>
        /// duplicate the values of this matrix into a new one
        /// </summary>
        /// <returns>copied matrix</returns>
        public MatrixEntity Duplicate()
        {
            MatrixEntity matrix = new MatrixEntity(this._row, this._col);
            for (int i = 0; i < this._row; i++)
                for (int j = 0; j < this._col; j++)
                    matrix[i, j] = this._Mat[i, j];
            return matrix;
        }

        /// <summary>
        /// Power matrix to exponent
        /// </summary>
        /// <param name="m"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static MatrixEntity Power(MatrixEntity m, int pow)
        {
            if (pow == 0) return Identity(m.Rows, m.Cols);
            if (pow == 1) return m.Duplicate();
            if (pow == -1) return Inverse(m);

            MatrixEntity x;
            if (pow < 0) { x = Inverse(m); pow *= -1; }
            else x = m.Duplicate();

            MatrixEntity ret = Identity(m.Rows, m.Cols);
            while (pow != 0)
            {
                if ((pow & 1) == 1) ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
        }
        #endregion

        #region Inverse and LU decomposition
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static MatrixEntity Minor(MatrixEntity input, int row, int col)
        {
            MatrixEntity mm = new MatrixEntity(input.Rows - 1, input.Cols - 1);
            int ii = 0;
            int jj = 0;
            for (int r = 0; r < input.Rows; r++)
            {
                if (r == row) continue;
                jj = 0;
                for (int c = 0; c < input.Cols; c++)
                {
                    if (c == col) continue;
                    mm[ii, jj] = input[r, c];
                    jj++;
                }
                ii++;
            }
            return mm;
        }

        /// <summary>
        /// this method determines the value of determinant using recursion
        /// </summary>
        /// <param name="input">matrix input</param>
        /// <returns> the value</returns>
        public static int Determinant(MatrixEntity input)
        {
            //if (!input.IsSquare()) throw new MatrixException("Matrix must be square to calculate determinant");
            //else
            //{
                if (input._row > 2)
                {
                    int value = 0;
                    for (int j = 0; j < input._row; j++)
                    {
                        MatrixEntity Temp = CreateSmalleMatrixEntity(input, 0, j);
                        value = value + input[0, j] * (SignOfElement(0, j) * Determinant(Temp));
                    }
                    return value;
                }
                else if (input._row == 2)
                {
                    return ((input[0, 0] * input[1, 1]) - (input[1, 0] * input[0, 1]));
                }
                else
                {
                    return (input[0, 0]);
                }
            //}
        }

        public static MatrixEntity Adjoint(MatrixEntity m)
        {
            if (!m.IsSquare())
            {
                throw new ArgumentOutOfRangeException("Dimension", m.Rows,
                   "The matrix must be square!");
            }
            MatrixEntity ma = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    ma[r, c] = (int)Math.Pow(-1, r + c) * Determinant(Minor(m, r, c));
                }
            }
            return ma.Transpose();
        }

        public static MatrixEntity Inverse(MatrixEntity m)
        {
            if (m.IsSingular())
            {
                throw new DivideByZeroException("Cannot inverse a matrix with a zero determinant!");
            }
            return Adjoint(m) / Determinant(m);
        }

        #endregion

        #region Operator overloading and stuff

        /// <summary>
        /// unary plus operator
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static MatrixEntity operator +(MatrixEntity m)
        {
            return m;
        }

        /// <summary>
        /// unary minus operator
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static MatrixEntity operator -(MatrixEntity m)
        {
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Cols; j++)
                {
                    m[i, j] = -m[i, j];
                }
            }
            return m;
        }

        /// <summary>
        /// arithmetic plus operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>sum of matrix a and b</returns>
        public static MatrixEntity operator +(MatrixEntity a, MatrixEntity b)
        {
            a.CheckMatrixDimensions(b);
            MatrixEntity res = new MatrixEntity(a._row, a._col);
            for (int r = 0; r < a._row; r++)
            {
                for (int c = 0; c < a._col; c++)
                {
                    res[r, c] = a[r, c] + b[r, c];
                }
            }
            return res;
        }

        /// <summary>
        /// arithmetic scalar plus operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>sum of matrix m and real number d</returns>
        public static MatrixEntity operator +(MatrixEntity m, int d)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = m[r, c] + d;
                }
            }
            return result;
        }

        /// <summary>
        /// arithmetic scalar plus operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>sum of real number d and matrix m</returns>
        public static MatrixEntity operator +(int d, MatrixEntity m)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = m[r, c] + d;
                }
            }
            return result;
        }

        /// <summary>
        /// arithmetic min operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>difference of matrix a and b</returns>
        public static MatrixEntity operator -(MatrixEntity a, MatrixEntity b)
        {
            a.CheckMatrixDimensions(b);
            MatrixEntity res = new MatrixEntity(a._row, a._col);
            for (int r = 0; r < a._row; r++)
            {
                for (int c = 0; c < a._col; c++)
                {
                    res[r, c] = a[r, c] - b[r, c];
                }
            }
            return res;
        }

        /// <summary>
        /// arithmetic scalar min operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>difference of matrix m and the real number d</returns>
        public static MatrixEntity operator -(MatrixEntity m, int d)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = m[r, c] - d;
                }
            }
            return result;
        }

        /// <summary>
        /// arithmetic scalar min operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>difference of the real number d and the matrix m</returns>
        public static MatrixEntity operator -(int d, MatrixEntity m)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = d - m[r, c];
                }
            }
            return result;
        }

        /// <summary>
        /// arithmetic multiply operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>multiplication of matrix a and b</returns>
        public static MatrixEntity operator *(MatrixEntity a, MatrixEntity b)
        {
            return a.Multiply(b);
        }

        /// <summary>
        /// arithmetic scalar multiply operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>multiplication of a real number a and a matrix b</returns>
        public static MatrixEntity operator *(int a, MatrixEntity b)
        {
            MatrixEntity res = new MatrixEntity(b._row, b._col);
            for (int r = 0; r < b._row; r++)
            {
                for (int c = 0; c < b._col; c++)
                {
                    res[r, c] = a * b[r, c];
                }
            }
            return res;
        }

        /// <summary>
        /// arithmetic scalar multiply operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>multiplication of matrix m and ab</returns>
        public static MatrixEntity operator *(MatrixEntity m, int d)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = m[r, c] * d;
                }
            }
            return result;
        }

        /// <summary>
        /// scalar divide operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns>a matrix with every element divided by the real d</returns>
        public static MatrixEntity operator /(MatrixEntity m, int d)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = m[r, c] / d;
                }
            }
            return result;
        }

        /// <summary>
        /// scalar divide operator
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <returns>a matrix with real d divided by every element </returns>
        public static MatrixEntity operator /(int d, MatrixEntity m)
        {
            MatrixEntity result = new MatrixEntity(m.Rows, m.Cols);
            for (int r = 0; r < m.Rows; r++)
            {
                for (int c = 0; c < m.Cols; c++)
                {
                    result[r, c] = d / m[r, c];
                }
            }
            return result;
        }

        #endregion

        //#region String stuff

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    String s = "\n";
        //    for (int r = 0; r < this._row; r++)
        //    {
        //        s += "| ";
        //        for (int c = 0; c < this._col; c++)
        //        {
        //            //s += String.Format("{0:F4} ", _Mat[r, c]);F4 laat 0 weg bij decimals==>geen align op punt
        //            s += _Mat[r, c].ToString("0.0000").PadLeft(12);
        //        }
        //        s += "|\n";
        //    }
        //    return s;
        //}

        //public static string NormalizeMatrixString(string matStr)   // From Andy - thank you! :)
        //{
        //    // Remove any multiple spaces
        //    while (matStr.IndexOf("  ") != -1)
        //        matStr = matStr.Replace("  ", " ");

        //    // Remove any spaces before or after newlines
        //    matStr = matStr.Replace(" \r\n", "\r\n");
        //    matStr = matStr.Replace("\r\n ", "\r\n");

        //    // If the data ends in a newline, remove the trailing newline.
        //    // Make it easier by first replacing \r\n’s with |’s then
        //    // restore the |’s with \r\n’s
        //    matStr = matStr.Replace("\r\n", "|");
        //    while (matStr.LastIndexOf("|") == (matStr.Length - 1))
        //        matStr = matStr.Substring(0, matStr.Length - 1);

        //    matStr = matStr.Replace("|", "\r\n");
        //    return matStr.Trim();
        //}

        //public static MMatrix Parse(string ps)                        // Function parses the matrix from string
        //{
        //    string s = NormalizeMatrixString(ps);
        //    string[] rows = Regex.Split(s, "\r\n");
        //    string[] nums = rows[0].Split(' ');
        //    MMatrix matrix = new MMatrix(rows.Length, nums.Length);
        //    try
        //    {
        //        for (int i = 0; i < rows.Length; i++)
        //        {
        //            nums = rows[i].Split(' ');
        //            for (int j = 0; j < nums.Length; j++) matrix[i, j] = double.Parse(nums[j]);
        //        }
        //    }
        //    catch (FormatException exc) { MessageBox.Show(exc.Message); }//"Wrong input format!"
        //    return matrix;
        //}

        //#endregion

    }
}

